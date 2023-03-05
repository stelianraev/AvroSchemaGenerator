using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace AutomatizationVersionUpdate
{
    public class AvroField
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public dynamic Type { get; set; }

        [JsonProperty("default")]
        public dynamic Default { get; set; } = "ignore";


        /// <summary>
        /// Ignore field "Default" when is not need for Serialization
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeDefault()
        {
            if(Default != null)
            {
                if (Default.GetType().Name == typeof(String).Name)
                {
                    return (Default != "ignore");
                }
            }

            return true;
        }
    }
}
