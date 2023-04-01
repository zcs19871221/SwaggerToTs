using System.Text.RegularExpressions;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.OpenAPIElements;

public class ResponseObject : ReferenceObject
{

  public string Description { get; set; }
  public Dictionary<string, HeaderObject>? Headers { get; set; }

  public Dictionary<string, MediaTypeObject>? Content { get; set; }
}