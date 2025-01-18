using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.CommonModels;

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
            using (var client = new HttpClient())
            {
                var number = _scenarioContext.Get<int>("number");
                var urlBase = _scenarioContext.Get<string>("urlBase");
                var url = $"{urlBase}api/Calculator/";
                var api_call = $"{url}number_attribute?number={number}";
                var response = client.GetAsync(api_call).Result;
                response.EnsureSuccessStatusCode();
                var responseBody = response.Content.ReadAsStringAsync().Result;
                var jsonDocument = JsonDocument.Parse(responseBody);
                var sqrt = jsonDocument.RootElement.GetProperty("squareRoot");

                if (sqrt.ValueKind == JsonValueKind.Number)
                    _scenarioContext.Add("squareRoot", sqrt.GetDouble());
                else if (sqrt.ValueKind == JsonValueKind.String && sqrt.GetString() == "NaN")
                    _scenarioContext.Add("squareRoot", double.NaN);
            }
        }

        [Then(@"the result would be (.*)")]
        public void ThenTheResultWouldBe(double expected)
        {
            var result = _scenarioContext.Get<double>("squareRoot");
            Assert.Equal(expected, result);
        }
    }
}