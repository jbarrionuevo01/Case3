using Microsoft.Playwright;
using System;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace calculator.frontend.tests.steps
{
	[Binding]
	public class SquareRootSteps
	{
		private readonly ScenarioContext _scenarioContext;

		public SquareRootSteps(ScenarioContext scenarioContext)
		{
			_scenarioContext = scenarioContext;
		}

		[Given(@"a number for square root (.*)")]
		public async Task GivenANumberForSquareRoot(int number)
		{
			_scenarioContext.Add("number", number);
		}

		[When(@"I calculate its square root")]
		public async Task WhenICalculateItsSquareRoot()
		{
			IPage page = _scenarioContext.Get<IPage>("page");
			var base_url = _scenarioContext.Get<string>("urlBase");
			var number = _scenarioContext.Get<int>("number");
			await page.GotoAsync($"{base_url}/Attribute");
			await page.FillAsync("#number", number.ToString());
			await page.ClickAsync("#attribute");
		}

		[Then(@"the result would be (.*)")]
		public async Task ThenTheResultWouldBe(string expectedResult)
		{
			var page = (IPage)_scenarioContext["page"];
			var resultText = await page.InnerTextAsync("#sqrt");
			var americanDouble = expectedResult.Replace(",", ".");
			var latinDouble = expectedResult.Replace(".", ",");

			var isValid = resultText.Equals(expectedResult) ||
						  resultText.Equals(americanDouble) ||
						  resultText.Equals(latinDouble);

			Assert.True(isValid, $"Expected result {expectedResult}, but got {resultText}");
		}
	}
}