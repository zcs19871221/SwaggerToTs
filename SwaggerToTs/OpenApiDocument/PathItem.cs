namespace SwaggerToTs.OpenApiDocument;

public class PathItem : TsCodeElement
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

  public string? Url { get; set; }

  protected override void ValidateOpenApiDocument()
  {
  }

  protected override TsCodeElement CreateTsCode()
  {
    if (Url == null) throw new Exception("url should not be null");

    var operations = new Dictionary<string, OperationObject?>
    {
      { "Get", Get },
      { "Put", Put },
      { "Post", Post },
      { "Delete", Delete },
      { "Options", Options },
      { "Head", Head },
      { "Patch", Patch },
      { "Trace", Trace }
    };
    var filteredOperations = new Dictionary<string, OperationObject>();
    foreach (var (method, operationObject) in operations)
      if (operationObject != null)
      {
        if (!TsCodeWriter.Get().Match(operationObject)) continue;

        if (TsCodeWriter.Get().Ignore(operationObject)) continue;

        operationObject.SetExportNameAndFileLocate(Url, method, TsCodeWriter.Get().OperationExportNames);
        ParameterObject.Append(Parameters, operationObject.Parameters);

        filteredOperations.Add(method.ToUpperInvariant(), operationObject);
      }

    return Merge(CreateFragment(filteredOperations));
  }
}