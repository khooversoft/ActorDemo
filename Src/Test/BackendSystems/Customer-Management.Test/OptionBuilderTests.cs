using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Customer_Management.Test
{
    public class OptionBuilderTests
    {
        [Fact]
        public void TestOptionBuilder_WhenDeployIsPassed_DeployShouldBeTrue()
        {
            var args = new string[]
            {
                "deploy"
            };

            IOption option = new OptionBuilder()
                .Build(args);

            option.Deploy.Should().BeTrue();
        }

        [Fact]
        public void TestOptionBuilder_WhenDeployIsTrue_DeployShouldBeTrue()
        {
            var args = new string[]
            {
                "deploy=true"
            };

            IOption option = new OptionBuilder()
                .Build(args);

            option.Deploy.Should().BeTrue();
        }

        [Fact]
        public void TestOptionBuilder_WhenDeployAndCustomerCount_CustomerCountAndDeployBe20AndTrue()
        {
            var args = new string[]
            {
                "deploy",
                "customercount=20"
            };

            IOption option = new OptionBuilder()
                .Build(args);

            option.Deploy.Should().BeTrue();
            option.CustomerCount.Should().Be(20);
        }
    }
}
