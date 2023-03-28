using SwaggerToTs.TypeScriptGenerator;

namespace SwaggerToTs.OpenAPIElements;

public class MediaTypeObject : TsCodeElement
{
  public SchemaObject? Schema { get; set; }

  public bool IsFromResponse { get; set; }
  //encoding todo

  protected override void ValidateOpenApiDocument()
  {
  }

  protected override TsCodeElement CreateTsCode()
  {
    ReadOnly = true;
    return Schema != null ? Schema.GenerateTsCode() : this;
  }
}