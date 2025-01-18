using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace calculator.frontend.Controllers
{
    public class CalculatorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
		// the base_url is obtained from environment variable
		// CALCULATOR_BACKEND_URL. If it is not present, it uses
		// "https://jbarrionuevo01-calculator-backend-uat.azurewebsites.net";
		private string base_url = 
            Environment.GetEnvironmentVariable("CALCULATOR_BACKEND_URL") ??
			"https://jbarrionuevo01-calculator-backend-uat.azurewebsites.net";
        const string api = "api/Calculator";
        private double ExecuteOperation(string operation, double num1, double num2)
        {
            var result = 0.0;
            var clientHandler = new HttpClientHandler();
            var client = new HttpClient(clientHandler);
            var url = $"{base_url}/{api}/{operation}?a={num1}&b={num2}";
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url),
            };
            using (var response = client.Send(request))
            {
                response.EnsureSuccessStatusCode();
                var body = response.Content.ReadAsStringAsync().Result;
                var json = JObject.Parse(body);
                var result_json = json["result"];
                if (result_json != null)
                {
                    result = result_json.Value<double>();
                }
            }
            return result;
        }
        [HttpPost]
        public ActionResult Index(string firstNumber, string secondNumber, string operation)
        {
            double num1 = Convert.ToDouble(firstNumber);
            double num2 = Convert.ToDouble(secondNumber);
            ViewBag.Result = ExecuteOperation(operation.ToLower(), num1, num2);
            return View();
        }
    }
}
