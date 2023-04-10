using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaHandlers;

public abstract class SchemaObjectHandler
{
  protected readonly Controller Controller;

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
  
  public abstract bool IsMatch(SchemaObject schema);

  protected abstract ValueSnippet DoConstruct(SchemaObject schema);

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

  protected SchemaObjectHandler(Controller controller)
  {
    Controller = controller;
  }
}