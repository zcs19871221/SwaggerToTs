namespace SwaggerToTs.OpenAPIElements;

public class OperationObject
{
  
  public OperationObject(Dictionary<string, ResponseObject>? responses)
  {
    Responses = responses ?? throw new Exception("should not be null");
  }

  public string? Description { get; set; }
  
  public string? Summary { get; set; }
  public bool? Deprecated { get; set; }
  
  public List<string> Tags { get; set; } = new();

  public string? OperationId { get; set; }

  public List<ParameterObject> Parameters { get; set; } = new();
  public RequestBodyObject? RequestBody { get; set; }

  public Dictionary<string, ResponseObject> Responses { get; set; }
  
}