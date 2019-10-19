using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GrainInterfaces
{
    public class FizzBuzzService : IFizzBuzzService
    {
        private readonly string _clusterId;
        private readonly string _serviceId;
        private readonly bool _addConsole;
        private IClusterClient _clusterClient;

        public FizzBuzzService(string clusterId, string serviceId, bool addConsole = false)
        {
            _clusterId = clusterId;
            _serviceId = serviceId;
            _addConsole = addConsole;
        }

        public async Task<IFizzBuzzService> Connect()
        {
            if (string.IsNullOrWhiteSpace(_clusterId)) throw new ArgumentNullException(nameof(_clusterId));
            if (string.IsNullOrWhiteSpace(_serviceId)) throw new ArgumentNullException(nameof(_serviceId));

            var builder = new ClientBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = _clusterId;
                    options.ServiceId = _serviceId;
                });

            if (_addConsole)
            {
                builder.ConfigureLogging(logging => logging.AddConsole());
            }

            _clusterClient = builder.Build();

            await _clusterClient.Connect();

            return this;
        }

        public Task<string> GetFizzBuzz(int value)
        {
            IFizzBuzz fizzBuzz = _clusterClient.GetGrain<IFizzBuzz>(0);
            return fizzBuzz.Evaluate(value);
        }
    }
}
