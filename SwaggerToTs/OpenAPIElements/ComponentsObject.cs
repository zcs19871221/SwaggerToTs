namespace SwaggerToTs.OpenAPIElements;

public class ComponentsObject
{
  public Dictionary<string, SchemaObject>? Schemas { get; set; }

  public Dictionary<string, ResponseObject>? Responses { get; set; }

  public Dictionary<string, ParameterObject>? Parameters { get; set; }

  public Dictionary<string, RequestBodyObject>? RequestBodies { get; set; }

  public Dictionary<string, HeaderObject>? Headers { get; set; }
  
}