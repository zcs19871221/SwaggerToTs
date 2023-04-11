
namespace SwaggerToTs.OpenAPIElements;

public class ParameterObject : ReferenceObject
{
  public string Name;
  public string In { get; set; }
  
  public string? Description { get; set; }
  
  public bool Required { get; set; }

  public bool Deprecated { get; set; }
  
  public bool AllowEmptyValue { get; set; }
  
  public string? Style { get; set; }

  public bool Explode { get; set; }

  public bool AllowReserved { get; set; }


  public Dictionary<string, MediaTypeObject>? Content { get; set; }

  public SchemaObject? Schema { get; set; }
}