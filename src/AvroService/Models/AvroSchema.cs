using Newtonsoft.Json;

namespace AutomatizationVersionUpdate.AvroService.Models
{
    public class AvroSchema
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("namespace")]
        public string Namespace { get; set; }

        [JsonProperty("fields")]
        public List<AvroField> Fields { get; set; }

        [JsonProperty("default")]
        public dynamic Default { get; set; }


        /// <summary>
        /// When avro is generated as class field property "Default" is useless and MUST be ignored for Serialization
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeDefault()
        {
            if (Default != null)
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
