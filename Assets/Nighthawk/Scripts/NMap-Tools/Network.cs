using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nmap_tools
{
    /// <summary>
    /// Represents a class-C network (i.e. 192.168.50.x or 192.168.50.0/24).
    /// </summary>
    public sealed class Network : CollectionBase
    {

        public Network() : base()
        {

        }

        public Network(Host host)
            : this()
        {
            this.Add(host);
        }

        public int Add(Host host)
        {
            if(Belongs(host))
                return List.Add(host);

            return -1;
        }

        public bool Contains(Host host)
        {
            return List.Contains(host);
        }

        public bool Belongs(Host host)
        {
            if(Count == 0)
                return true;

            string subnet1 = this.ToString();
            string subnet2 = Network.GetSubnet(host);

            return subnet1.Equals(subnet2, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Attempts to return the class-C subnet/network for the given Host.
        /// </summary>
        /// <param name="host">Host</param>
        /// <returns>string</returns>
        public static string GetSubnet(Host host)
        {
            return host.Addresses[0].Name.Substring(0, host.Addresses[0].Name.LastIndexOf('.') + 1);
        }

        public Host this[int index]
        {
            get { return (Host)List[index]; }
            set { List[index] = value; }
        }

        public override string ToString()
        {
            if (List.Count > 0)
                return this[0].Addresses[0].Name.Substring(0, this[0].Addresses[0].Name.LastIndexOf('.') + 1);

            return "(n/a)";
        }
    }
}
