using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaHandlers;

public abstract class SchemaObjectHandler
{
  protected Controller Controller;

  protected void AddCommonComments(ValueSnippet snippet, SchemaObject schema)
  {
    snippet.AddComments(new[]
    {
      (nameof(schema.Description), schema.Description),
      (nameof(schema.Title), schema.Title),
      (nameof(schema.Deprecated), schema.Deprecated ? "True": ""),
      (nameof(schema.Format), schema.Format)
    });
  }
  
  protected void SetNullValue(ValueSnippet snippet, SchemaObject schema)
  {
    snippet.IsNullable = schema.Nullable;
  }
  abstract public bool IsMatch(SchemaObject schema);

  abstract public ValueSnippet DoConstruct(SchemaObject schema);

  protected virtual void SetNullable(ValueSnippet snippet, SchemaObject schema)
  {
    snippet.IsNullable = schema.Nullable;
  }
  
  public ValueSnippet Construct(SchemaObject schema)
  {
    var snippet = DoConstruct(schema);
    snippet.AddComments(new[]
    {
      (nameof(schema.Description), schema.Description),
      (nameof(schema.Title), schema.Title),
      (nameof(schema.Deprecated), schema.Deprecated ? "True": ""),
      (nameof(schema.Format), schema.Format)
    });
    SetNullable(snippet, schema);
    return snippet;
  }

  public SchemaObjectHandler(Controller controller)
  {
    Controller = controller;
  }
}