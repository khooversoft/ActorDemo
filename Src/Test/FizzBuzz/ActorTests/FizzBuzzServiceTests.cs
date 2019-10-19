using FluentAssertions;
using GrainInterfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ActorTests
{
    public class FizzBuzzServiceTests
    {
        [Fact]
        public async Task TestGuardOnService_WhenInvalidClusterId_ShouldRaiseException()
        {
            var service = new FizzBuzzService(null, "serviceId");

            Func<Task> act = async () => { await service.Connect(); };

            await act.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task TestGuardOnService_WhenInvalidServiceId_ShouldRaiseException()
        {
            var service = new FizzBuzzService("clusterId", "");

            Func<Task> act = async () => { await service.Connect(); };

            await act.Should().ThrowAsync<ArgumentNullException>();
        }
    }
}
