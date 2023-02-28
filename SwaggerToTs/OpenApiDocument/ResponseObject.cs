namespace SwaggerToTs.OpenApiDocument;

public class ResponseObject : TsCodeElement
{
  public Dictionary<string, HeaderObject>? Headers { get; set; }

  public Dictionary<string, MediaTypeObject>? Content { get; set; }

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
        CreateFragment(Headers, onItemGenerated: (_, item, parent) => { parent.Optional = item.Optional; }));

    if (Content != null)
    {
      response.Add("Content", CreateFragment(Content));
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

    return Merge(CreateFragment(response));
  }
}