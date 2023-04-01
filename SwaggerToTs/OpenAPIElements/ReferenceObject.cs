using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.OpenAPIElements;

public class ReferenceObject
{
  [JsonProperty(PropertyName = "$ref")] public string? Reference { get; set; }
  
  public string? ExportName { get; set; }
}