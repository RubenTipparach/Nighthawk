using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Nighthawk.Scripts.Searchsploit_Tools
{
    [JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    public class SearchsploitJson
    {
        [JsonProperty]
        public string SEARCH;

        [JsonProperty]
        public string DB_PATH_EXPLOIT;

        [JsonProperty]
        public SploitResults[] RESULTS_EXPLOIT;

        [JsonProperty]
        public string DB_PATH_SHELLCODE;

        [JsonProperty]
        public SploitResults[] RESULTS_SHELLCODE;

    }

    [JsonObject(MemberSerialization.OptIn)]
    [Serializable]
    public class SploitResults
    {
        [JsonProperty]
        public string Title;

        [JsonProperty(PropertyName = "EBD-ID")]
        public string EBDID;

        [JsonProperty]
        public string Date;

        [JsonProperty]
        public string Author;

        [JsonProperty]
        public string Type;

        [JsonProperty]
        public string Platform;

        [JsonProperty]
        public string Path;
    }
}
