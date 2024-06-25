using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CanConverter
{
    internal class EmtronData
    {
            public int Id { get; set; }

            public string Name { get; set; }

            public string ByteOffset { get; set; }

            public string Unit { get; set; } = "user";

            public int DecimalPlaces { get; set; } = 1;

            public int Multiplier { get; set; } = 1;

            public int Divider { get; set; } = 1;

            public int Offset { get; set; } = 0;

            public string Type { get; set; } = "u16-le";
    }
}
