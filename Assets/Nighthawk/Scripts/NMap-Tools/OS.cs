using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace nmap_tools
{
    /// <summary>
    /// Represents an OS element within the NMAPRUN/HOST element(s).
    /// </summary>
    public sealed class OS
    {
        [XmlArrayItem("portused")]
        public List<PortUsed> PortsUsed { get; set; }

        /// <summary>
        /// Fingerprint
        /// </summary>
        [XmlElement("osfingerprint")]
        public OsFingerprint OsFingerprint { get; set; }

        /// <summary>
        /// This could be NULL when the OS could not be determined.
        /// </summary>
        [XmlElement("osmatch")]
        public OsMatch Match { get; set; }

        public override string ToString()
        {
            return (Match != null) ? Match.Name : "Unknown";
        }
    }

    public sealed class OsMatch  
    {
        [XmlArrayItem("osclass")]
        public List<OsClass> Classes { get; set; }

        /// <summary>
        /// Name and possibly version of the OS detected.
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("accuracy")]
        public int Accuracy { get; set; }

        [XmlAttribute("line")]
        public int Line { get; set; }
    }

    public sealed class OsClass
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("vendor")]
        public string Vendor { get; set; }

        [XmlAttribute("osfamily")]
        public string Family { get; set; }

        [XmlAttribute("osgen")]
        public string Generation { get; set; }

        [XmlAttribute("accuracy")]
        public int Accuracy { get; set; }

        [XmlElement("cpe")]
        public CPE CPE { get; set; }

        public override string ToString()
        {
            return Type + " " + Vendor + " (" + Generation + "; " + Accuracy + "%)";
        }
    }

    public sealed class CPE
    {
        [XmlText]
        public string Value { get; set; }

        public override string ToString()
        {
            return Value;
        }
    }

    public sealed class PortUsed
    {
        [XmlAttribute("state")]
        public string State { get; set; }

        [XmlAttribute("portid")]
        public int Id { get; set; }

        [XmlAttribute("proto")]
        public string Protocol { get; set; }

        public override string ToString()
        {
            return Protocol + "/" + Id + " (" + State + ")";
        }

    }

    public sealed class OsFingerprint
    {
        [XmlAttribute("fingerprint")]
        public string Fingerprint { get; set; }

        public override string ToString()
        {
            return Fingerprint;
        }

    }
}
