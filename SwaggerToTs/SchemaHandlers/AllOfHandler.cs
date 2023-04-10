using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.SchemaSnippets;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaHandlers;

public class AllOfHandler : SchemaObjectHandler
{
  public override bool IsMatch(SchemaObject schema)
  {
    return schema.Allof.Any();
  }

  protected override ValueSnippet DoConstruct(SchemaObject schema)
  {
    var allOfs = schema.Allof.Select(Controller.SchemaObjectHandlerWrapper.Construct).ToList();
    if (Controller.SchemaObjectHandlerWrapper.ObjectHandler.IsMatch(schema))
    {
      allOfs.Add(Controller.SchemaObjectHandlerWrapper.ObjectHandler.Construct(schema));
    }

    return new AllOfSnippet(allOfs);
  }

  public AllOfHandler(Controller controller) : base(controller)
  {
  }
}