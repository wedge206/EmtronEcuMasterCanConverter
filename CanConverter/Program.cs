using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;

namespace CanConverter
{
    internal class Program
    {
        private static List<EmtronData> emtronData;

        static void Main(string[] args)
        {
            var fileList = new Dictionary<string, string>() { { "Tx1", "0x200" }, { "Tx2", "0x300" }, { "Tx3", "0x400" }, { "Tx4", "0x500" } };

            foreach (var file in fileList)
            {
                string canId = "0x310";
                string sourceFile = $"C:\\Users\\matt\\OneDrive\\FocusRally\\Emtron\\Dataset Files\\Custom{file.Key}.edf";

                emtronData = JsonSerializer.Deserialize<List<EmtronData>>(File.OpenRead("EmtronTypeList.json"));
                var channelList = File.ReadAllText(sourceFile).Split(',').Select(p => int.Parse(p)).Where(p => p != 0).ToList();

                var prog = new Program();
                prog.Execute(file.Key, file.Value, channelList);
            }
        }

        public void Execute(string canName, string canId, List<int> channelList)
        {
            var mob1 = new MessageObject()
            {
                Id = $"Emtron{canName}",
                CanbusId = canId,
                Width = "1"
            };

            var byteOffset = 0;
            var frameOffset = 0;
            var frame = new Frame()
            {
                Id = $"frame +{frameOffset}",
                Offset = frameOffset.ToString()
            };

            foreach (var channelId in channelList)
            {
                var item = GetCanxChannel(channelId);
                var name = item.Name.Replace(' ', '_');

                if (string.IsNullOrWhiteSpace(name))
                {
                    name = channelId.ToString();
                }

                var channel = new Channel()
                {
                    Id = $"c_{name}",
                    ByteOffset = byteOffset.ToString(),
                    Type = "u16-le",
                    Offset = item.Offset,
                    DecimalPlaces = item.DecimalPlaces,
                    Multiplier = item.Multiplier,
                    Divider = item.Divider,
                    Unit = item.Unit
                };

                byteOffset += 2;

                frame.Channels.Add(channel);

                if (frame.Channels.Count == 4)
                {
                    mob1.Frames.Add(frame);

                    byteOffset = 0;
                    frameOffset++;

                    frame = new Frame()
                    {
                        Id = $"frame +{frameOffset}",
                        Offset = frameOffset.ToString()
                    };
                }
            }

            if (frame.Channels.Count > 0 && frame.Channels.Count < 4)
            {
                mob1.Frames.Add(frame);
            }

            var result = new CanBusXport();
            result.MobList.Add(mob1);

            mob1.Width = mob1.Frames.Count().ToString();

            if (mob1.Frames.Count() > 8)
            {
                //TODO: Split extra frames across multiple message objects
            }
            else if (mob1.Frames.Count() > 4)
            {
                mob1.Width = "8";
            }
            else if (mob1.Frames.Count() == 3)
            {
                mob1.Width = "4";
            }

            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            var serializer = new XmlSerializer(typeof(CanBusXport));
            using (var stream = new FileStream($"C:\\Users\\matt\\OneDrive\\FocusRally\\Ecumaster\\Emtron{canName}.canx", FileMode.Create))
            using (var writer = new XmlTextWriter(stream, System.Text.Encoding.UTF8))
            {
                writer.Formatting = Formatting.Indented;
                serializer.Serialize(writer, result, ns);
            }

           // File.Copy("test.xml", "test.xml.canx", true);
        }

        private Channel GetCanxChannel(int id)
        {
            if (!emtronData.Any(p => p.Id == id))
            {
                throw new Exception($"Missing ID: {id}");
            }

            var item = emtronData.Single(p => p.Id == id);
            
            return new Channel()
            {
                Id = item.Id.ToString(),
                Type = item.Type,
                DecimalPlaces = item.DecimalPlaces,
                Divider = item.Divider,
                Multiplier = item.Multiplier,
                Offset = item.Offset,
                Unit = item.Unit,
                Name = item.Name,
            };
        }
    }
}