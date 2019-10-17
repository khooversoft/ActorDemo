using Orleans;
using Orleans.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using GrainInterfaces;
using System.Linq;
using System.Collections.Generic;

namespace Client
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            return await new Program().Run(args);
        }

        private async Task<int> Run(string[] args)
        {
            try
            {
                using (var client = await ConnectClient())
                {
                    await DoClientWork(client);
                    Console.ReadKey();
                }

                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine($"\nException while trying to run client: {e.Message}");
                Console.WriteLine("Make sure the silo the client is trying to connect to is running.");
                Console.WriteLine("\nPress any key to exit.");
                Console.ReadKey();
                return 1;
            }
        }

        private async Task<IClusterClient> ConnectClient()
        {
            IClusterClient client;
            client = new ClientBuilder()
                .UseLocalhostClustering()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = "dev";
                    options.ServiceId = "OrleansBasics";
                })
                .ConfigureLogging(logging => logging.AddConsole())
                .Build();

            await client.Connect();
            Console.WriteLine("Client successfully connected to silo host \n");
            return client;
        }

        private static async Task DoClientWork(IClusterClient client)
        {
            // example of calling grains from the initialized client
            var fizzBuzz = client.GetGrain<IFizzBuzz>(0);

            var valueList = new List<string>();

            foreach (var testValue in Enumerable.Range(0, 100))
            {
                string evaluation = await fizzBuzz.Evaluate(testValue);
                valueList.Add($"{testValue,3}: {evaluation,-8}");

                if (valueList.Count == 10)
                {
                    Console.WriteLine(string.Join(", ", valueList));
                    valueList.Clear();
                }
            }

            Console.WriteLine(string.Join(", ", valueList));
        }
    }
}
