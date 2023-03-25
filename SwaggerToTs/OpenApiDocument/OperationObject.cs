using System.Text.RegularExpressions;

namespace SwaggerToTs.OpenApiDocument;

public class OperationObject : TsCodeElement
{
  static public readonly string EndWith = "EP";


  public OperationObject(Dictionary<string, ResponseObject>? responses)
  {
    Responses = responses ?? throw new Exception("should not be null");
    Priority = -1;
  }

  public List<string> Tags { get; } = new();

  public string? OperationId { get; set; }

  public List<ParameterObject> Parameters { get; } = new();
  public RequestBodyObject? RequestBody { get; set; }

  public Dictionary<string, ResponseObject> Responses { get; set; }

  public string? ExportNameBase { get; set; }
  public void SetExportNameAndFileLocate(string url, string method, HashSet<string> allOperationName)
  {
    var fileName = Tags.FirstOrDefault() ?? url.Split("/").ToList()
      .Find(e => !string.IsNullOrWhiteSpace(e) && !e.Contains("{") && !e.Contains("api"));
    if (string.IsNullOrWhiteSpace(fileName)) throw new Exception("operation FileName should not be empty");
    var exportName = !string.IsNullOrWhiteSpace(OperationId)
      ? OperationId
      : $"{fileName}{method}";
    exportName = ToPascalCase(exportName) + EndWith;
    while (!allOperationName.Add(exportName))
    {
      if (!Regex.IsMatch(exportName, "(\\d+)$"))
      {
        exportName += 1;
        continue;
      }

      exportName = Regex.Replace(exportName, "(\\d+)$", m => (int.Parse(m.Groups[1].ToString()) + 1).ToString());
    }

    ExportName = exportName;
    ExportNameBase = Regex.Replace(exportName, $"{EndWith}(\\d*)$", "$1");
    FileLocate = $"{fileName}";
  }

  protected override void ValidateOpenApiDocument()
  {
    if (Responses.Count == 0) throw new Exception("responses must have one item");

    if (!string.IsNullOrEmpty(OperationId))
      if (!TsCodeWriter.Get().OperationIds.Add(OperationId))
        throw new Exception($"has Duplicate OperationId: {OperationId}");


    foreach (var statusCode in Responses.Keys)
      if (!Regex.Match(statusCode, @"^[1-5][\dx]{2}$").Success)
        throw new Exception($"not valid statusCode: {statusCode}");
  }

  protected override TsCodeElement CreateTsCode()
  {
    var requestParameters = new Dictionary<string, TsCodeElement>();
    ParameterObject.MergeRequestParameters(requestParameters, this);
    RequestBodyObject.MergeRequestBody(requestParameters, this);


    return Merge(CreateFragment(new Dictionary<string, TsCodeElement>
    {
      { "Request", CreateFragment(requestParameters) }, { "Responses",  CreateFragment(Responses, (s, o) =>
      {
        var wrapper = new TsCodeFragment
        {
          Optional = false,
          Name = s
        };
        o.OperationObject = this;
        o.StatusCode = s;
        return wrapper.Merge(o.GenerateTsCode());
      }) }
    })).ExtractTo();
  }
}