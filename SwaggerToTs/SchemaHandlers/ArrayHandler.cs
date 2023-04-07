using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.SchemaSnippets;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaHandlers;

public class ArrayHandler : SchemaObjectHandler
{
  public override bool IsMatch(SchemaObject schema)
  {
    return schema.Type == "array";
  }


  public override ValueSnippet Construct(SchemaObject schema)
  {
    return new ArraySnippet(schema, Controller);
  }

  public ArrayHandler(Controller controller) : base(controller)
  {
  }
}