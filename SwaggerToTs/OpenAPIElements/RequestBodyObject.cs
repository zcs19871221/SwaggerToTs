
namespace SwaggerToTs.OpenAPIElements;

public class RequestBodyObject : ReferenceObject
{
  public Dictionary<string, MediaTypeObject> Content { get; set; }

  public string? Description { get; set; }

  public bool Required { get; set; }
  
}