using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Media;

namespace SWApp.Helpers
{
    public class RALToRGBConverter
    {
        public class RalColor
        {
            public string Name { get; set; }
            public int R { get; set; }
            public int G { get; set; }
            public int B { get; set; }

            public Brush RgbColor => new SolidColorBrush(Color.FromRgb((byte)R, (byte)G, (byte)B));
        }
        
        public class RalColorConverter
        {
            private readonly Dictionary<string, RalColor> _ralColors;
            private readonly string RALtoRGBFilepath = "C:\\Users\\ebabs\\source\\repos\\SWAddinByBS\\SWApp\\assets\\RAL_Colors.json";

            public RalColorConverter()
            {
                _ralColors = JsonConvert.DeserializeObject<Dictionary<string, RalColor>>(File.ReadAllText(RALtoRGBFilepath));
            }

            public RalColor GetColorByRalName(string ralName)
            {
                if (_ralColors.TryGetValue(ralName, out var color))
                {
                    return color;
                }
                else
                {
                    throw new KeyNotFoundException($"RAL code {ralName} not found.");
                }
            }

            public ObservableCollection<RalColor> GetColors()
            {
                ObservableCollection<RalColor> allColors = new ObservableCollection<RalColor>();

                var ralColorsDict = JsonConvert.DeserializeObject<Dictionary<string, RalColor>>(File.ReadAllText(RALtoRGBFilepath));

                // Convert dictionary to ObservableCollection<RalColor>

                foreach (var entry in ralColorsDict)
                {
                    // Set the RAL code for each color
                    entry.Value.Name = entry.Key;
                    allColors.Add(entry.Value);
                }

                return allColors;
            }


        }
    }
}