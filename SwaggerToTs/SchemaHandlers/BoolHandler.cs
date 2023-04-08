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

  public override ValueSnippet Construct(SchemaObject schema)
  {
    return new BoolSnippet(schema);
  }


  public BoolHandler(Controller controller) : base(controller)
  {
  }
}