using Newtonsoft.Json;
using System.Diagnostics.Metrics;

namespace CounterApp.Models
{
    internal class Counter
    {
        public string Filename { get; set; }
        public string Name { get; set; }
        public int DefaultValue { get; set; }
        public int Value { get; set; }
        public DateTime Date { get; set; }
        public string ColorHex { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public static Counter FromFile(string filename)
        {
            string counterSerialized = File.ReadAllText(filename);
            Counter counter;
            
            try
            {
                counter = JsonConvert.DeserializeObject<Counter>(counterSerialized);
            }
            catch (JsonSerializationException)
            {

                counter = new Counter
                {
                    Name = "Licznik",
                    DefaultValue = 0,
                    Value = 0,
                    ColorHex = Colors.Transparent.ToHex(),
                    Date = File.GetLastWriteTime(filename)
                };
            }

            counter.Filename = filename;
            return counter;
        }

        public void Save()
        {
            File.WriteAllText(Filename, ToJson());
        }

    }
}