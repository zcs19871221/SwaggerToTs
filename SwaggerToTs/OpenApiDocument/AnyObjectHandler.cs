namespace SwaggerToTs.OpenApiDocument;

public class AnyObjectHandler : ISchemaHandler
{
  public bool IsMatch(SchemaObject schema)
  {
    return schema.Type == "object" && !schema.Properties.Any() && !schema.Allof.Any();
  }


  public void CreateTsCode(SchemaObject schema)
  {
    schema.SchemaType = SchemaTypeEnums.Object;
    schema.AddComment(nameof(schema.MinProperties), schema.MinProperties.ToString())
      .AddComment(nameof(schema.MaxProperties), schema.MaxProperties.ToString());
    schema.Contents = @"Record<string, unknown>";
  }
}