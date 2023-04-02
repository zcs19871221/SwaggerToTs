using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaSnippets;

public class UnknownHandler : ISchemaHandler
{
  public bool IsMatch(SchemaObject schema)
  {
    return true;
  }


  public ValueSnippet CreateTsCode(SchemaObject schema)
  {
    schema.SchemaType = SchemaTypeEnums.Any;
    schema.Nullable = false;
    schema.Contents = "unknown";
  }
}