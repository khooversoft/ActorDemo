using System;
using System.Threading.Tasks;

namespace Customer_Management
{
    class Program
    {
        const int _Ok = 0;
        const int _Error = 1;

        static async Task<int> Main(string[] args)
        {
            return await new Program().Run(args);
        }

        private Task<int> Run(string[] args)
        {
            return Task.FromResult(_Ok);
        }
    }
}
