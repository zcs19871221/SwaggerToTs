using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.SchemaSnippets;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaHandlers;

public class ObjectHandler: SchemaObjectHandler
{
  public override bool IsMatch(SchemaObject schema)
  {
    return schema.Type?.ToLower() == "object";
  }

  public override ValueSnippet Construct(SchemaObject schema)
  {
    return new ObjectSnippet(schema, Controller);
  }


  public ObjectHandler(Controller controller) : base(controller)
  {
  }
}