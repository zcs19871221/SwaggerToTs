using Newtonsoft.Json;

namespace SwaggerToTs.OpenAPIElements;

public class ReferenceObject
{
  [JsonProperty(PropertyName = "$ref")] public string? Reference { get; set; }
}