using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.SchemaSnippets;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaHandlers;

public class OneOfHandler : SchemaObjectHandler
{
  public override bool IsMatch(SchemaObject schema)
  {
    return schema.Oneof.Any();
  }

  public override ValueSnippet Construct(SchemaObject schema)
  {
    return new OneOfSnippet(schema, Controller);
  }

  public OneOfHandler(Controller controller) : base(controller)
  {
  }
}