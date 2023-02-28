namespace SwaggerToTs.OpenApiDocument;

public class MediaTypeObject : TsCodeElement
{
  public SchemaObject? Schema { get; set; }

  //encoding todo

  protected override void ValidateOpenApiDocument()
  {
  }

  protected override TsCodeElement CreateTsCode()
  {
    ReadOnly = true;
    if (Schema != null) return Schema.GenerateTsCode();

    return this;
  }
}