namespace SwaggerToTs.OpenApiDocument;

public class TsCodeFragment : TsCodeElement
{
  protected override void ValidateOpenApiDocument()
  {
    // throw new NotImplementedException();
  }

  protected override TsCodeElement CreateTsCode()
  {
    return this;
  }
}