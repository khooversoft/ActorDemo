using FluentAssertions;
using GrainInterfaces;
using Orleans.TestingHost;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ActorTests
{
    public class FizzBuzzSiloTests: IClassFixture<ClusterFixture>
    {
        private readonly TestCluster _cluster;

        public FizzBuzzSiloTests(ClusterFixture fixture)
        {
            _cluster = fixture.Cluster;
        }

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
            var actor = _cluster.GrainFactory.GetGrain<IFizzBuzz>(0);
            string result = actor.Evaluate(testValue).GetAwaiter().GetResult();
            result.Should().Be(shouldBeValue);
        }
    }
}
