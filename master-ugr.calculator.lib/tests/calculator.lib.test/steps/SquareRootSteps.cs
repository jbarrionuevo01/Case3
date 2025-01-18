using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace calculator.lib.test.steps
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
        public void GivenANumber(int number)
        {
            _scenarioContext.Add("number", number);
        }

        [When(@"I calculate its square root")]
        public void WhenICalculateItsSquareRoot()
        {
            var number = _scenarioContext.Get<int>("number");
            try
            {
                var squareRoot = NumberAttributter.SquareRoot(number);
                _scenarioContext.Add("result", squareRoot);
            }
            catch (ArgumentException)
            {
                _scenarioContext.Add("result", "error");
            }
        }

        [Then(@"the result would be (.*)")]
        public void ThenTheResultWouldBe(double expected)
        {
            var result = _scenarioContext.Get<double>("result");
            Assert.Equal(expected, result);
        }
    }
}