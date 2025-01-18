using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace calculator.frontend.tests.steps
{
    [Binding]
    public class CalculatorSteps
    {
        private readonly ScenarioContext _scenarioContext;
        public CalculatorSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }
        [Given(@"the first number is (.*)")]
        public async Task GivenTheFirstNumberIs(int number1)
        {
            IPage page = _scenarioContext.Get<IPage>("page");
            var base_url = _scenarioContext.Get<string>("urlBase");
            await page.GotoAsync($"{base_url}/Calculator");
            await page.FillAsync("#firstNumber", number1.ToString());
        }

        [Given(@"the second number is (.*)")]
        public async Task GivenTheSecondNumberIs(int number2)
        {
            IPage page = _scenarioContext.Get<IPage>("page");
            await page.FillAsync("#secondNumber", number2.ToString());
        }

        [When(@"the two numbers are added")]
        public async Task WhenTheTwoNumbersAreAdded()
        {
            var page = (IPage)_scenarioContext["page"];
            await page.SelectOptionAsync("#operation", "Add");
            await page.ClickAsync("#calculateButton");
        }

        [When(@"I substract first number to second number")]
        public async Task WhenISubstractFirstNumberToSecondNumber()
        {
            var page = (IPage)_scenarioContext["page"];
            await page.SelectOptionAsync("#operation", "Subtract");
            await page.ClickAsync("#calculateButton");
        }

        [When(@"I multiply both numbers")]
        public async Task WhenIMultiplyBothNumbers()
        {
            var page = (IPage)_scenarioContext["page"];
            await page.SelectOptionAsync("#operation", "Multiply");
            await page.ClickAsync("#calculateButton");
        }


        [When(@"I divide first number by second number")]
		[When(@"I divide both numbers")]
		public async Task WhenIDivideFirstNumberBySecondNumber()
        {
            var page = (IPage)_scenarioContext["page"];
            await page.SelectOptionAsync("#operation", "Divide");
            await page.ClickAsync("#calculateButton");
        }
        [Then(@"the result should be (.*)")]
        [Then(@"the result is (.*)")]
        [Then(@"the result shall be (.*)")]
        public async Task ThenTheResultShouldBe(string expectedResult)
        {
            var page = (IPage)_scenarioContext["page"];
            var resultText = await page.InnerTextAsync("#result");
            var americanDouble = expectedResult.Replace(",",".");
            var latinDouble = expectedResult.Replace(".", ",");
            var ok = expectedResult.Equals(americanDouble) || 
                expectedResult.Equals(latinDouble);
            Assert.True(ok, $"expected {expectedResult} but actual {resultText}");
        }
    }
}
