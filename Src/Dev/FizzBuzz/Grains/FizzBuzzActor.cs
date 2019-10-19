using GrainInterfaces;
using Orleans;
using Orleans.Concurrency;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grains
{
    [StatelessWorker]
    [Reentrant]
    public class FizzBuzzActor : Grain, IFizzBuzz
    {
        private static List<Func<int, string>> _evaulations = new List<Func<int, string>>
        {
            x => x == 0 ? x.ToString() : null,
            x => x % 3 == 0 && x % 5 == 0 ? "FizzBuzz" : null,
            x => x % 3 == 0 ? "Fizz" : null,
            x => x % 5 == 0 ? "Buzz" : null,
            x => x.ToString(),
        };

        public Task<string> Evaluate(int value)
        {
            string result = _evaulations
                .Select(x => x.Invoke(value))
                .SkipWhile(x => x == null)
                .First();

            return Task.FromResult(result);
        }
    }
}
