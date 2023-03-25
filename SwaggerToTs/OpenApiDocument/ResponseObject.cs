namespace SwaggerToTs.OpenApiDocument;

public class ResponseObject : TsCodeElement
{
  public OperationObject? OperationObject { get; set; }
  public Dictionary<string, HeaderObject>? Headers { get; set; }

  public Dictionary<string, MediaTypeObject>? Content { get; set; }
  public string StatusCode { get; set; } = null!;

  protected override void ValidateOpenApiDocument()
  {
    if (!string.IsNullOrWhiteSpace(Reference)) return;
    if (string.IsNullOrEmpty(Description)) throw new Exception("Response Description should not be null or empty");
  }

  protected override TsCodeElement CreateTsCode()
  {
    Dictionary<string, TsCodeElement> response = new();
    if (Headers != null)
      response.Add("Headers",
        CreateFragment(Headers, (key, item) =>
        {
          var header = item.GenerateTsCode();
          var wrapper = new TsCodeFragment()
          {
            Name = key,
            Optional = header.Optional
          };
          return wrapper.Merge(header);
        }));

    if (Content != null)
    {
      var content = CreateFragment(dict: Content,  (contentType, item) =>
      {
        var wrapper = new TsCodeFragment
        {
          Name = contentType,
          Optional = false,
        };
        var media = item.GenerateTsCode();
        if (OperationObject != null)
        {
          if (string.IsNullOrWhiteSpace(media.ExportName))
          {
            media.ExtractTo(OperationObject.ExportName?.Replace(OperationObject.EndWith, "") + StatusCode + ToPascalCase(contentType[(contentType.IndexOf("/", StringComparison.Ordinal) + 1)..]), OperationObject.FileLocate);
          }

          media = media.NonNullAsRequired();
        }
        return wrapper.Merge(media);
      });
      response.Add("Content", content);
    }
    else
    {
      var content = new TsCodeFragment
      {
        Contents = "null",
        Optional = false
      };
      response.Add("Content", content);
    }
  
    return Merge(CreateFragment(dict:response));
  }
}