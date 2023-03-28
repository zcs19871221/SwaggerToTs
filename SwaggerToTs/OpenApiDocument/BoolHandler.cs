namespace SwaggerToTs.OpenApiDocument;

public class BoolHandler : ISchemaHandler
{
  public bool IsMatch(SchemaObject schema)
  {
    return schema.Type == "boolean";
  }


  public void CreateTsCode(SchemaObject schema)
  {
    schema.SchemaType = SchemaTypeEnums.Bool;
    schema.Contents = "boolean";
  }
}