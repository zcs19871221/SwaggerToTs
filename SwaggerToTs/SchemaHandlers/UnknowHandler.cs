using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.SchemaSnippets;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaHandlers;

public class UnknownHandler: SchemaObjectHandler
{
  public override bool IsMatch(SchemaObject schema)
  {
    return true;
  }

  protected override void SetNullable(ValueSnippet snippet, SchemaObject schema)
  {
    snippet.IsNullable = false;
  }

  protected override ValueSnippet DoConstruct(SchemaObject schema)
  {
    return new UnknownSnippet();
  }


  public UnknownHandler(Controller controller) : base(controller)
  {
  }
}