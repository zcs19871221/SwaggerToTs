namespace SwaggerToTs.OpenAPIElements;

public class OpenApiObject
{
  public OpenApiObject(SortedDictionary<string, PathItemObject>? paths, Info? info, string? openApi)
  {
    if (paths == null || info == null || openApi == null)
    {
      throw new Exception("should not be null");
    }
    Paths = paths;
    Info = info;
    OpenApi = openApi;
  }

  public string OpenApi { get; set; }
  public SortedDictionary<string, PathItemObject> Paths { get; set; }
  public Info Info { get; set; }

  public ComponentsObject? Components { get; set; }

}

public class Info
{
  public string? Title { get; set; }
  public string? Description { get; set; }
  public string? Version { get; set; }
}