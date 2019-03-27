using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace nmap_tools
{
    /// <summary>
    /// Represents a PORT element within a NMAPRUN/HOST element(s).
    /// </summary>
    public sealed class Port
    {
        /// <summary>
        /// Protocol (i.e. tcp, udp, etc.)
        /// </summary>
        [XmlAttribute("protocol")]
        public string Protocol { get; set; }

        /// <summary>
        /// Port number
        /// </summary>
        [XmlAttribute("portid")]
        public int Id { get; set; }

        /// <summary>
        /// State
        /// </summary>
        [XmlElement("state")]
        public State State { get; set; }

        /// <summary>
        /// Service information if available.
        /// </summary>
        [XmlElement("service")]
        public Service Service { get; set; }

        public override string ToString()
        {
            return Protocol + "/" + Id + " (" + State + ")";
        }

    }

    public sealed class State
    {
        [XmlAttribute("state")]
        public string Value { get; set; }

        [XmlAttribute("reason")]
        public string Reason { get; set; }

        [XmlAttribute("reason_ttl")]
        public int ReasonTTL { get; set; }

        public override string ToString()
        {
            return Value;
        }

    }

    public sealed class Service
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("method")]
        public string Method { get; set; }

        [XmlAttribute("conf")]
        public int Conf { get; set; }

        public override string ToString()
        {
            return Name;
        }

    }
}
