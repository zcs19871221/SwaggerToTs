using SwaggerToTs.TypeScriptGenerator;

namespace SwaggerToTs.OpenAPIElements;

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

  private const string BodyName = "Body";

  public static void MergeRequestBody(IDictionary<string, TsCodeElement> requestParameters, OperationObject operation)
  {
    if (operation.RequestBody != null)
    {
      requestParameters.Add(BodyName, operation.RequestBody.GenerateTsCode());
    }
  }
}