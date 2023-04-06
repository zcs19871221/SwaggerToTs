using SwaggerToTs.Handlers;
using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaSnippets;

public class AnyOfHandler : SchemaObjectHandler
{
  public override bool IsMatch(SchemaObject schema)
  {
    return schema.AnyOf.Any();
  }

  public override ValueSnippet Construct(SchemaObject schema)
  {
    return new AnyOfSnippet(schema, Controller);
  }
  
  public AnyOfHandler(Controller controller) : base(controller)
  {
  }
}