using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Nighthawk.Scripts.Searchsploit_Tools
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SearchsploitJson
    {
        [JsonProperty]
        public string SEARCH { get; set; }

        [JsonProperty]
        public string DB_PATH_EXPLOIT{ get; set; }

        [JsonProperty]
        public SploitResults[] RESULTS_EXPLOIT { get; set; }

        [JsonProperty]
        public string DB_PATH_SHELLCODE { get; set; }

        [JsonProperty]
        public SploitResults[] RESULTS_SHELLCODE { get; set; }

    }

    [JsonObject(MemberSerialization.OptIn)]
    public class SploitResults
    {
        [JsonProperty]
        public string Title { get; set; }

        [JsonProperty(PropertyName ="EBD-ID")]
        public string EBDID { get; set; }

        [JsonProperty]
        public string Date{ get; set; }

        [JsonProperty]
        public string Author{ get; set; }

        [JsonProperty]
        public string Type { get; set; }

        [JsonProperty]
        public string Platform { get; set; }

        [JsonProperty]
        public string Path { get; set; }
    }
}
