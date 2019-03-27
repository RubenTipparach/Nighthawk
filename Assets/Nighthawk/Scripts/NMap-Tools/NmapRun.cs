using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace nmap_tools
{
    /// <summary>
    /// Represents a NMAPRUN root element.
    /// </summary>
    [XmlRoot("nmaprun")]
    public sealed class NmapRun
    {
        /// <summary>
        /// Name of the scanner which produced this report. Normally this is
        /// nmap.
        /// </summary>
        [XmlAttribute("scanner")]
        public string Scanner { get; set; }

        /// <summary>
        /// Arguments passed to nmap binary when running the scan.
        /// </summary>
        [XmlAttribute("args")]
        public string Arguments { get; set; }

        /// <summary>
        /// Unix Timestamp when the scan was started.
        /// </summary>
        [XmlAttribute("start")]
        public int StartTime { get; set; }
        
        /// <summary>
        /// Locale string of timestamp created on the system where the 
        /// scan was performed.
        /// </summary>
        [XmlAttribute("startstr")]
        public string StartTimeStr { get; set; }

        /// <summary>
        /// Version of Nmap used within this NMAPRUN.
        /// </summary>
        [XmlAttribute("version")]
        public string Version { get; set; }

        [XmlAttribute("xmloutputversion")]
        public string XmlOutputVersion { get; set; }

        /// <summary>
        /// Collection of HOST elements within the NMAPRUN. These are the hosts within the
        /// Nmap scan.
        /// </summary>
        [XmlElement("host")]
        public List<Host> Hosts { get; set; }

        [XmlElement("scaninfo")]
        public ScanInfo ScanInfo { get; set; }

        [XmlElement("verbose")]
        public Verbose Verbose { get; set; }

        [XmlElement("debugging")]
        public Debugging Debugging { get; set; }

        [XmlElement("runstats")]
        public RunStats RunStats { get; set; }

        /// <summary>
        /// Parses the given file (at path) and hopefully returns a NmapRun object.
        /// </summary>
        /// <param name="path">Path to the file to parse.</param>
        /// <returns>NmapRun; null on errors in parsing.</returns>
        public static NmapRun Parse(string path)
        {
            NmapRun run = null;
            try
            {
                XmlSerializer s = new XmlSerializer(typeof(NmapRun));
                run = (NmapRun)s.Deserialize(File.OpenRead(path));
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: Could not parse file '" + path + "': " + e.GetType() + " " + e.Message);
            }

            return run;
        }

        /// <summary>
        /// Saves the NmapRun object to a File at the given path.
        /// </summary>
        /// <param name="path">Path to save the NmapRun object too.</param>
        /// <returns>Whether or not the save was successful.</returns>
        public bool Save(string path)
        {
            bool ok = false;
            try
            {
                XmlSerializer s = new XmlSerializer(typeof(NmapRun));
                s.Serialize(File.OpenWrite(path), this);
                ok = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: Could not save NmapRun object to path {0}: {1} {2}", path, e.GetType().Name, e.Message);
            }

            return ok;
        }

        /// <summary>
        /// Replaces the given target Host object with the replacement Host object given.
        /// </summary>
        /// <param name="target">Target Host object to remove.</param>
        /// <param name="replacement">Replacement Host object to add.</param>
        public void ReplaceHost(Host target, Host replacement)
        {
            Hosts.Remove(target);
            Hosts.Add(replacement);
        }
    }

    public sealed class ScanInfo
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("protocol")]
        public string Protocol { get; set; }

        [XmlAttribute("numservices")]
        public int NumServices { get; set; }

        [XmlAttribute("services")]
        public string Services { get; set; }

    }

    public sealed class Verbose
    {
        [XmlAttribute("level")]
        public int Level { get; set; }

    }

    public sealed class Debugging
    {
        [XmlAttribute("level")]
        public int Level { get; set; }

    }

    public sealed class RunStats
    {
        [XmlElement("finished")]
        public Finished Finished { get; set; }

        public int GetCountHostsUp()
        {
            return Finished.Hosts.Up;
        }

        public int GetCountHostsDown()
        {
            return Finished.Hosts.Down;
        }

        public int GetCountTotalHosts()
        {
            return Finished.Hosts.Total;
        }
    }

    public sealed class Finished
    {
        [XmlAttribute("time")]
        public int Time { get; set; }

        [XmlAttribute("timestr")]
        public string TimeStr { get; set; }

        [XmlAttribute("summary")]
        public string Summary { get; set; }

        [XmlElement("hosts")]
        public Hosts Hosts { get; set; }

    }

    public sealed class Hosts
    {
        [XmlAttribute("up")]
        public int Up { get; set; }

        [XmlAttribute("down")]
        public int Down { get; set; }

        [XmlAttribute("total")]
        public int Total { get; set; }

    }

}
