using System.Xml.Serialization;

namespace CanConverter
{
    [XmlRoot("CANbuseXport")]
    public class CanBusXport
    {
        [XmlElement("mob")]
        public List<MessageObject> MobList { get; set; } = new List<MessageObject>();
    }

    public class MessageObject
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("ext")]
        public string Ext { get; set; } = "0";

        [XmlAttribute("canbusID")]
        public string CanbusId { get; set; }

        [XmlAttribute("width")]
        public string Width { get; set; }

        [XmlElement("frame")]
        public List<Frame> Frames { get; set; } = new List<Frame>();
    }

    public class Frame
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("offset")]
        public string Offset { get; set; }

        [XmlElement("channel")]
        public List<Channel> Channels { get; set; } = new List<Channel>();
    }

    public class Channel
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("byteOffset")]
        public string ByteOffset { get; set; }

        [XmlAttribute("unit")]
        public string Unit { get; set; } = "user";

        [XmlAttribute("decimalPlaces")]
        public int DecimalPlaces { get; set; } = 1;

        [XmlAttribute("multiplier")]
        public int Multiplier { get; set; } = 1;

        [XmlAttribute("divider")]
        public int Divider { get; set; } = 1;

        [XmlAttribute("offset")]
        public int Offset { get; set; } = 0;

        [XmlAttribute("type")]
        public string Type { get; set; } = "u16-le";
    }
}
