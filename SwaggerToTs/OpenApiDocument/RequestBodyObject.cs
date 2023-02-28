namespace SwaggerToTs.OpenApiDocument;

public class RequestBodyObject : TsCodeElement
{
  public Dictionary<string, MediaTypeObject>? Content { get; set; }

  public RequestBodyObject(bool required = false)
  {
    Optional = !required;
  }

  protected override void ValidateOpenApiDocument()
  {
  }

  protected override TsCodeElement CreateTsCode()
  {
    if (Content != null)
    {
      return Merge(CreateFragment(Content));
    }

    return this;
  }

  private static string _bodyName = "Body";
  public static void MergeRequestBody(IDictionary<string, TsCodeElement> requestParameters, OperationObject operation)
  {
    if (operation.RequestBody != null)
    {
      requestParameters.Add(_bodyName, operation.RequestBody.GenerateTsCode());
    }
  }
}