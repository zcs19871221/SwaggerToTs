namespace SwaggerToTs.OpenAPIElements;

public class ResponseObject : ReferenceObject
{
  public ResponseObject(string description)
  {
    Description = description;
  }

  public string Description { get; set; }
  public Dictionary<string, HeaderObject>? Headers { get; set; }

  public Dictionary<string, MediaTypeObject>? Content { get; set; }
  
}