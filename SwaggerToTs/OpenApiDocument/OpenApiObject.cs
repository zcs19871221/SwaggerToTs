using System.Text.RegularExpressions;

namespace SwaggerToTs.OpenApiDocument;

public class OpenApiObject : TsCodeElement
{
  public OpenApiObject(SortedDictionary<string, PathItem>? paths, Info? info, string? openApi)
  {
    if (paths == null || info == null || openApi == null) throw new Exception("should not to null");

    Paths = paths;
    Info = info;
    OpenApi = openApi;
  }

  public string OpenApi { get; set; }
  public SortedDictionary<string, PathItem> Paths { get; set; }
  public Info Info { get; set; }

  public ComponentsObject? Components { get; set; }

  protected override void ValidateOpenApiDocument()
  {
    List<string> checkDup = new();
    foreach (var (requestUrl, pathItem) in Paths)
    {
      if (!requestUrl.StartsWith('/')) throw new Exception("not correct path:" + requestUrl);

      var unifiedKey = Regex.Replace(requestUrl, "{[^}]+}", "{_key}");
      if (checkDup.Contains(unifiedKey)) throw new Exception("paths contains dup path:" + unifiedKey);

      pathItem.Url = requestUrl;
      checkDup.Add(unifiedKey);
    }
  }

  protected override TsCodeElement CreateTsCode()
  {
    var exportName = "Routes";
    AddComment(nameof(OpenApi), OpenApi)
      .AddComment(nameof(Info.Description), Info.Description)
      .AddComment(nameof(Info.Title), Info.Title)
      .AddComment(nameof(Info.Version), Info.Version);
    var content = CreateFragment(Paths);
    return Merge(content).ExtractTo(exportName, exportName);
  }
}

public class Info
{
  public string? Title { get; set; }
  public string? Description { get; set; }
  public string? Version { get; set; }
}