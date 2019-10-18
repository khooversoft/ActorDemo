using Orleans;
using Orleans.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using GrainInterfaces;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;

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

        private static Task DoClientWork(IClusterClient client)
        {
            // example of calling grains from the initialized client
            var fizzBuzz = client.GetGrain<IFizzBuzz>(0);

            const int numberOfCallsToMake = 10000;

            var valueList = new (int testValue, string evalulation)[numberOfCallsToMake];
            int valueListIndex = valueList.GetLowerBound(0) - 1;
            var taskList = new List<Task>(numberOfCallsToMake);

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            foreach (var testValue in Enumerable.Range(0, numberOfCallsToMake))
            {
                Task t = fizzBuzz.Evaluate(testValue).ContinueWith(x =>
                {
                    int nextIndex = Interlocked.Increment(ref valueListIndex);
                    valueList[nextIndex] = (testValue, x.Result);
                });

                taskList.Add(t);
            }

            Task.WaitAll(taskList.ToArray());
            stopwatch.Stop();

            Console.WriteLine($"Number of calls {numberOfCallsToMake}, timeMs {stopwatch.ElapsedMilliseconds}");

            int lineCount = 0;
            var grouping = valueList
                .Select(x => $"{x.testValue,4}: {x.evalulation,-8}")
                .GroupBy(x => lineCount++ / 10)
                .Select(x => string.Join(", ", x));

            foreach (var item in grouping)
            {
                Console.WriteLine(item);
            }

            return Task.FromResult(0);
        }
    }
}
