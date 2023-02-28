namespace SwaggerToTs.OpenApiDocument;

public class HeaderObject : ParameterObject
{

  public HeaderObject(bool required = false):base(required)
  {
  }
  protected override void ValidateOpenApiDocument()
  {
    if (!string.IsNullOrWhiteSpace(Name) || !string.IsNullOrWhiteSpace(In))
      throw new Exception("header object should not be set name");
    ValidContent();
  }
}