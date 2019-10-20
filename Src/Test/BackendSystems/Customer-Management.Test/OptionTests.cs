using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Customer_Management.Test
{
    public class OptionTests
    {
        [Fact]
        public void VerifyNonValidParameter_WhenInvalidNameIsTrue_ShouldThrowException()
        {
            var values = new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("notValid", "xxx")
            };

            Action act = () => new Option(values);

            act.Should().Throw<ArgumentException>();
        }
        
        [Fact]
        public void VerifyDeploy_WhenValuePassedIsTrue_ShouldBeTrue()
        {
            var values = new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("deploy", "true")
            };

            var subject = new Option(values);

            subject.Deploy.Should().BeTrue();
        }

        [Fact]
        public void VerifyDeploy_WhenValuePassedIsFalse_ShouldBeFalse()
        {
            var values = new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("deploy", "false")
            };

            var subject = new Option(values);

            subject.Deploy.Should().BeFalse();
        }

        [Fact]
        public void VerifyCustomerCount_WhenValuePassedTen_ShouldBe10()
        {
            var values = new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("customerCount", "10")
            };

            var subject = new Option(values);

            subject.CustomerCount.Should().Be(10);
        }

        [Fact]
        public void VerifyDeployCustomerCount_WhenDeployPassed15_ShouldBeTrueAnd15()
        {
            var values = new KeyValuePair<string, string>[]
            {
                new KeyValuePair<string, string>("customerCount", "15"),
                new KeyValuePair<string, string>("deploy", "true")
            };

            var subject = new Option(values);

            subject.CustomerCount.Should().Be(15);
        }
    }
}
