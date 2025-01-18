using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.CommonModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace calculator.lib.test.steps
{
    [Binding]
    public class NumberAttributeSteps
    {
        private readonly ScenarioContext _scenarioContext;

        public NumberAttributeSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }
        [When("number (.*) is checked for multiple attributes")]
        public void NumberIsCheckedForMultipleAttributes(int number)

        {
            using (var client = new HttpClient())
            {
                var urlBase = _scenarioContext.Get<string>("urlBase");
                var url = $"{urlBase}api/Calculator/";
                var api_call = $"{url}number_attribute?number={number}";
                var response = client.GetAsync(api_call).Result;
                response.EnsureSuccessStatusCode();
                var responseBody = response.Content.ReadAsStringAsync().Result;
                var jsonDocument = JsonDocument.Parse(responseBody);
                var odd = jsonDocument.RootElement.GetProperty("odd").GetBoolean();
                var prime = jsonDocument.RootElement.GetProperty("prime").GetBoolean();
                var sqrt = jsonDocument.RootElement.GetProperty("squareRoot");
                _scenarioContext.Add("isOdd", odd);
                _scenarioContext.Add("isPrime", prime);

                if (sqrt.ValueKind == JsonValueKind.Number)
                    _scenarioContext.Add("squareRoot", sqrt.GetDouble());
                else if (sqrt.ValueKind == JsonValueKind.String && sqrt.GetString() == "NaN")
                       _scenarioContext.Add("squareRoot", double.NaN);
                
            }
        }

        [Then(@"the answer to know whether is prime or not is (.*)")]
        public void ThenTheAnswerToKnowWhetherIsPrimeOrNotIsTrue(bool isIt)
        {
            var isPrime = _scenarioContext.Get<bool>("isPrime");
            Assert.Equal(isPrime,isIt);
        }

        [Then(@"the answer to know whether is odd or not is (.*)")]
        public void ThenTheAnswerToKnowWhetherIsOddOrNotIsTrue(bool isIt)
        {
            var isOdd = _scenarioContext.Get<bool>("isOdd");
            Assert.Equal(isOdd, isIt);
        }

        [Then(@"the result of square root is (.*)")]
        public void ThenTheResultOfSqrtIs(double expected)
        {
            var sqrt = _scenarioContext.Get<double>("squareRoot");
            Assert.Equal(sqrt, expected);
        }
    }
}
