using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaHandlers;


public interface ISchemaHandler
{
  public bool IsMatch(SchemaObject schema);
  public ValueSnippet GenerateSnippet(SchemaObject schema);
}

public static class Helper
{
  public static void IniSchemaSnippet(ValueSnippet snippet, SchemaObject schema)
  {
    snippet.AddComments(new[]
    {
      (nameof(schema.Title), schema.Title),
      (nameof(schema.Description), schema.Description),
      (nameof(schema.Deprecated), schema.Deprecated ? "true": ""),
      (nameof(schema.Format), schema.Format),
    });
    snippet.IsNullable = schema.Nullable;
  }
}
