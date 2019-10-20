using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Customer_Management
{
    public class Option : IOption
    {
        private static readonly Dictionary<string, Action<Option, string>> _setActions = new Dictionary<string, Action<Option, string>>(StringComparer.OrdinalIgnoreCase)
        {
            [nameof(Deploy)] = (c, x) => c.Deploy = bool.Parse(x),
            [nameof(CustomerCount)] = (c, x) => c.CustomerCount = int.Parse(x),
        };

        public Option(IEnumerable<KeyValuePair<string, string>> values)
        {
            values = values ?? throw new ArgumentNullException(nameof(values));

            var notFound = values
                .Select(x => x.Key)
                .Except(_setActions.Keys, StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (notFound.Count > 0) throw new ArgumentException($"Parameter(s) {string.Join(", ", notFound)} are not valid");

            values
                .Join(_setActions, x => x.Key, x => x.Key, (o, i) => new { value = o, action = i }, StringComparer.OrdinalIgnoreCase)
                .ToList()
                .ForEach(x => x.action.Value(this, x.value.Value));
        }

        public bool Deploy { get; private set; }

        public int CustomerCount { get; private set; } = 100;
    }
}
