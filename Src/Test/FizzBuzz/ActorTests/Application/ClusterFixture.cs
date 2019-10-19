using GrainInterfaces;
using Grains;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Orleans.Hosting;
using Orleans.TestingHost;
using System;
using System.Collections.Generic;
using System.Text;

namespace ActorTests
{
    public class ClusterFixture : IDisposable
    {
        public ClusterFixture()
        {
            var builder = new TestClusterBuilder();
            builder.AddSiloBuilderConfigurator<TestSiloConfigurations>();
            Cluster = builder.Build();
            Cluster.Deploy();
        }

        public void Dispose()
        {
            Cluster.StopAllSilos();
        }

        public TestCluster Cluster { get; }
    }

    internal class TestSiloConfigurations : ISiloBuilderConfigurator
    {
        public void Configure(ISiloHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(FizzBuzzActor).Assembly).WithReferences());
        }
    }
}
