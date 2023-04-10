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

  public override ValueSnippet DoConstruct(SchemaObject schema)
  {
    var oneOfs = schema.Oneof.Select(Controller.SchemaObjectHandlerWrapper.Construct).ToList();
    return new OneOfSnippet(oneOfs);
  }
  

  public OneOfHandler(Controller controller) : base(controller)
  {
  }
}