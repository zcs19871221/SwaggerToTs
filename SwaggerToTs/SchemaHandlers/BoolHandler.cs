using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.SchemaSnippets;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaHandlers;

public class BoolHandler: SchemaObjectHandler
{
  public override bool IsMatch(SchemaObject schema)
  {
    return schema.Type?.ToLower() == "boolean";
  }

  protected override ValueSnippet DoConstruct(SchemaObject schema)
  {
    return new BoolSnippet();
  }


  public BoolHandler(Controller controller) : base(controller)
  {
  }
}