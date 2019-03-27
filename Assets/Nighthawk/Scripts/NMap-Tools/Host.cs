using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace nmap_tools
{
    /// <summary>
    /// Represents a Host found during an Nmap scan.
    /// </summary>
    public sealed class Host
    {
        [XmlAttribute("starttime")]
        public int StartTime { get; set; }

        [XmlAttribute("endtime")]
        public int EndTime { get; set; }

        [XmlElement("status")]
        public Status Status { get; set; }

        /// <summary>
        /// List of IPv4/6 addresses found by Nmap during the scan.
        /// </summary>
        [XmlElement("address")]
        public List<Address> Addresses { get; set; }

        /// <summary>
        /// List of Hostnames found by Nmap during the scan. However, Nmap
        /// uses either DNS settings given to it during the scan or the 
        /// system DNS setting which may/not be able to resolve all hosts.
        /// </summary>
        [XmlArray("hostnames")]
        [XmlArrayItem("hostname")]
        public List<Hostname> Hostnames { get; set; }

        /// <summary>
        /// List of Port objects for this Host.
        /// </summary>
        [XmlArray("ports")]
        [XmlArrayItem("port")]
        public List<Port> Ports { get; set; }

        /// <summary>
        /// The OS if the OS detection option was given for this scan. Be sure
        /// to check the DetectedOS() method to see whether or not Nmap was 
        /// able to resolve the OS fingerprint.
        /// </summary>
        [XmlElement("os")]
        public OS OS { get; set; }

        [XmlElement("uptime")]
        public UpTime UpTime { get; set; }

        [XmlElement("distance")]
        public Distance Distance { get; set; }

        [XmlElement("tcpsequence")]
        public TcpSequence TcpSequence { get; set; }

        [XmlElement("ipidsequence")]
        public IpIdSequence IpIdSequence { get; set; }

        [XmlElement("tcptssequence")]
        public TcpTsSequence TcpTsSequence { get; set; }

        [XmlElement("times")]
        public Times Times { get; set; }

        /// <summary>
        /// Returns only the IPv4 and IPv6 addresses for this Host.
        /// </summary>
        public string[] IpAddresses
        {
            get
            {
                if (Addresses.Count > 0)
                {
                    string ips = "";
                    foreach (Address addr in Addresses)
                    {
                        if (addr.Type.StartsWith("ipv", StringComparison.CurrentCultureIgnoreCase))
                        {
                            ips += addr.Name + ",";
                        }
                    }

                    return (ips.Length > 0) ? ips.Substring(0, ips.Length - 1).Split(',') : new string[0];
                }

                return new string[0];
            }
        }

        /// <summary>
        /// Returns the first IP address found for this Host.
        /// </summary>
        public string FirstIpAddress { get { return (IpAddresses.Length > 0) ? IpAddresses[0] : "";  } }

        /// <summary>
        /// Returns the MAC Address found for this Host.
        /// </summary>
        public string MacAddress
        {
            get
            {
                foreach (Address addr in Addresses)
                {
                    if (!addr.Type.StartsWith("ipv"))
                        return addr.Name;

                }

                return "";
            }
        }

        /// <summary>
        /// Returns TRUE if the OS was detected during this scan.
        /// </summary>
        /// <returns>bool</returns>
        public bool DetectedOS()
        {
            return (OS != null && OS.Match != null);
        }

        /// <summary>
        /// String representation of this Host object. If a Hostname is found, 
        /// we will default to that value. Otherwise, the first IP address is
        /// returned.
        /// </summary>
        /// <returns>string; Hostname and IP Address -- OR -- FirstIPAddress</returns>
        public override string ToString()
        {
            return (Hostnames.Count > 0) ? String.Format("{0} ({1})", Hostnames[0].Name, FirstIpAddress) : FirstIpAddress;
        }

    }

    public sealed class Status
    {
        [XmlAttribute("state")]
        public string State { get; set; }

        [XmlAttribute("reason")]
        public string Reason { get; set; }

    }

    public sealed class Address
    {
        [XmlAttribute("addr")]
        public string Name { get; set; }

        [XmlAttribute("addrtype")]
        public string Type { get; set; }

        [XmlAttribute("vendor")]
        public string Vendor { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    public sealed class Hostname
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }

        public override string ToString()
        {
            return Name;
        }

    }

    public sealed class UpTime
    {
        [XmlAttribute("seconds")]
        public int Seconds { get; set; }

        [XmlAttribute("lastboot")]
        public string LastBoot { get; set; }

    }

    public sealed class Distance
    {
        [XmlAttribute("value")]
        public int Value { get; set; }
    }

    public sealed class TcpSequence
    {
        [XmlAttribute("index")]
        public int Index { get; set; }

        [XmlAttribute("difficulty")]
        public string Difficulty { get; set; }

        [XmlAttribute("values")]
        public string Values { get; set; }

    }

    public sealed class IpIdSequence
    {
        [XmlAttribute("class")]
        public string Class { get; set; }

        [XmlAttribute("values")]
        public string Values { get; set; }

    }

    public sealed class TcpTsSequence
    {
        [XmlAttribute("class")]
        public string Class { get; set; }

        [XmlAttribute("values")]
        public string Values { get; set; }

    }

    public sealed class Times
    {
        [XmlAttribute("srtt")]
        public int Srtt { get; set; }

        [XmlAttribute("rttvar")]
        public int RttVar { get; set; }

        [XmlAttribute("to")]
        public int To { get; set; }

    }

}
