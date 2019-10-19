using FluentAssertions;
using Grains;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ActorTests
{
    public class FizzBuzzTests
    {
        [Theory]
        [InlineData(0, "0")]
        [InlineData(1, "1")]
        [InlineData(3, "Fizz")]
        [InlineData(4, "4")]
        [InlineData(5, "Buzz")]
        [InlineData(15, "FizzBuzz")]
        [InlineData(16, "16")]
        public void TestEvaulationForFizzBuzzForSamples(int testValue, string shouldBeValue)
        {
            var actor = new FizzBuzzActor();
            string result = actor.Evaluate(testValue).GetAwaiter().GetResult();
            result.Should().Be(shouldBeValue);
        }

        [Fact]
        public void TestLargeRangeWithCalculatedValues()
        {
            Func<int, string> caluclateValue = x =>
            {
                if (x == 0) return x.ToString();
                else if (x % 3 == 0 && x % 5 == 0) return "FizzBuzz";
                else if (x % 3 == 0) return "Fizz";
                else if (x % 5 == 0) return "Buzz";

                return x.ToString();
            };

            var actor = new FizzBuzzActor();

            Enumerable.Range(0, 100)
                .Select(x => new { testValue = x, shouldBeValue = caluclateValue(x), resultValue = actor.Evaluate(x).GetAwaiter().GetResult() })
                .All(x => x.resultValue == x.shouldBeValue)
                .Should().BeTrue();
        }
    }
}
