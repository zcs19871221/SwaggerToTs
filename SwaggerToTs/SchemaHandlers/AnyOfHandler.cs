using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.SchemaSnippets;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaHandlers;

public class AnyOfHandler : SchemaObjectHandler
{
  public override bool IsMatch(SchemaObject schema)
  {
    return schema.AnyOf.Any();
  }

  public override ValueSnippet DoConstruct(SchemaObject schema)
  {
    return new AnyOfSnippet(schema.AnyOf.Select(Controller.SchemaObjectHandlerWrapper.Construct).ToList());
  }
  
  public AnyOfHandler(Controller controller) : base(controller)
  {
  }
}