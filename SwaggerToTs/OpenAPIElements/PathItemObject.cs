using SwaggerToTs.Snippets;

namespace SwaggerToTs.OpenAPIElements;

public class PathItemObject
{
  public OperationObject? Get { get; set; }
  public OperationObject? Put { get; set; }
  public OperationObject? Post { get; set; }
  public OperationObject? Delete { get; set; }
  public OperationObject? Options { get; set; }
  public OperationObject? Head { get; set; }
  public OperationObject? Patch { get; set; }
  public OperationObject? Trace { get; set; }

  public List<ParameterObject> Parameters { get; } = new();

  public string? Description { get; set; }
  
  public string? Summary { get; set; }
  public string? Url { get; set; }

  
}