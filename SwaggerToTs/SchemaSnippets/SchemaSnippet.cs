using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaSnippets;


public abstract class SchemaSnippet: ValueSnippet
{
  private void _init(SchemaObject schema)
  {
    AddComments(new[]
    {
      (nameof(schema.Description), schema.Description),
      (nameof(schema.Title), schema.Title),
      (nameof(schema.Deprecated), schema.Deprecated ? "True": ""),
      (nameof(schema.Format), schema.Format),
    });
  }
  

  public SchemaSnippet(SchemaObject schema, bool? isNull = null)
  {
    _init(schema);
    IsNullable = isNull ?? schema.Nullable;
  }

}