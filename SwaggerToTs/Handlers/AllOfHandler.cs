using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.SchemaSnippets;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.Handlers;

public class AllOfHandler : SchemaObjectHandler
{
  public override bool IsMatch(SchemaObject schema)
  {
    return schema.Allof.Any();
  }

  public override ValueSnippet Construct(SchemaObject schema)
  {
    return new AllOfSnippet(schema, Controller);
  }


  public AllOfHandler(Controller controller) : base(controller)
  {
  }
}