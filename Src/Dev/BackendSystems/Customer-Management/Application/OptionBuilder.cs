using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Customer_Management
{
    public class OptionBuilder
    {
        public OptionBuilder()
        {
        }

        public IOption Build(string[] args)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();

            args
                .Select(x => x.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries))
                .Where(x => x.Length == 2 && x[0].Equals("ConfigFile", StringComparison.OrdinalIgnoreCase))
                .Select(x => x[1])
                .ToList()
                .ForEach(x => builder.AddJsonFile(x, optional: false));

            var arguments = args
                .Join(typeof(Option).GetProperties().Where(x => x.PropertyType == typeof(bool)), x => x, x => x.Name, (o, i) => i, StringComparer.OrdinalIgnoreCase)
                .Select(x => $"{x.Name}=true")
                .Concat(args)
                .ToArray();

            builder.AddCommandLine(arguments);

            IConfiguration configuration = builder.Build();

            var propertyValues = configuration
                .GetChildren()
                .Select(x => new KeyValuePair<string, string>(x.Key, x.Value))
                .ToArray();

            IOption option = new Option(propertyValues);

            if (!option.Deploy) throw new ArgumentException("Deploy");
            if (option.CustomerCount <= 0) throw new ArgumentException("CustomerCount must be greater then 0");

            return option;
        }
    }
}
