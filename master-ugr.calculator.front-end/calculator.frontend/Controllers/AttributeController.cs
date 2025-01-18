using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace calculator.frontend.Controllers
{
	public class AttributeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		private string base_url = Environment.GetEnvironmentVariable("CALCULATOR_BACKEND_URL") ??
		"https://jbarrionuevo01-calculator-backend-uat.azurewebsites.net";

		const string api = "api/Calculator";

		private async Task<Dictionary<string, string>> ExecuteOperationAsync(string number)
		{
			var result = new Dictionary<string, string>
			{
				{ "IsPrime", "unknown" },
				{ "IsOdd", "unknown" },
				{ "squareRoot", "unknown" }
			};

			try
			{
				if (string.IsNullOrWhiteSpace(number) || !int.TryParse(number, out _))
				{
					throw new ArgumentException("Invalid input: Please enter a valid number.");
				}

				var clientHandler = new HttpClientHandler();
				using (var client = new HttpClient(clientHandler))
				{
					var url = $"{base_url}/api/Calculator/number_attribute?number={number}";
					var request = new HttpRequestMessage
					{
						Method = HttpMethod.Get,
						RequestUri = new Uri(url),
					};

					var response = await client.SendAsync(request);
					response.EnsureSuccessStatusCode();
					var body = await response.Content.ReadAsStringAsync();
					var json = JObject.Parse(body);

					var raw_prime = json["prime"]?.Value<bool>();
					var raw_odd = json["odd"]?.Value<bool>();
					var raw_square = json["squareRoot"]?.Value<double>();

					result["IsPrime"] = raw_prime.HasValue ? (raw_prime.Value ? "Yes" : "No") : "unknown";
					result["IsOdd"] = raw_odd.HasValue ? (raw_odd.Value ? "Yes" : "No") : "unknown";
					result["squareRoot"] = raw_square.HasValue ? raw_square.Value.ToString() : "unknown";
				}
			}
			catch (HttpRequestException ex)
			{
				result["Error"] = $"HTTP error: {ex.Message}";
			}
			catch (ArgumentException ex)
			{
				result["Error"] = ex.Message;
			}
			catch (Exception ex)
			{
				result["Error"] = $"An unexpected error occurred: {ex.Message}";
			}

			return result;
		}

		[HttpPost]
		public async Task<ActionResult> Index(string number)
		{
			var result = await ExecuteOperationAsync(number);

			if (result.ContainsKey("Error"))
			{
				ViewBag.Error = result["Error"];
				ViewBag.IsPrime = "unknown";
				ViewBag.IsOdd = "unknown";
				ViewBag.Sqrt = "unknown";
			}
			else
			{
				ViewBag.IsPrime = result["IsPrime"];
				ViewBag.IsOdd = result["IsOdd"];
				ViewBag.Sqrt = result["squareRoot"];
			}

			return View();
		}
	}
}
