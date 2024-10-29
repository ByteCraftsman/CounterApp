using CounterApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CounterApp.Models
{
    internal class AllCounters
    {
        public ObservableCollection<Counter> Counters { get; set; } = new();
        public AllCounters() => LoadCounters();

        public void LoadCounters()
        {
            Counters.Clear();
            string appDataPath = FileSystem.AppDataDirectory;

            IEnumerable<Counter> counters = Directory
                .EnumerateFiles(appDataPath, "*.json")
                .Select(filename => Counter.FromFile(filename))
                .OrderByDescending(counter => counter.Date);

            foreach (Counter counter in counters)
            {
                Counters.Add(counter);
            }
        }
    }
}