using SwaggerToTs.OpenAPIElements;

namespace SwaggerToTs.SchemaHandlers;

public class UnknownHandler : ISchemaHandler
{
  public bool IsMatch(SchemaObject schema)
  {
    return true;
  }


  public void CreateTsCode(SchemaObject schema)
  {
    schema.SchemaType = SchemaTypeEnums.Any;
    schema.Nullable = false;
    schema.Contents = "unknown";
  }
}