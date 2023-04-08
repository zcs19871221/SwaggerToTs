using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.SchemaSnippets;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaHandlers;

public class NumberHandler: SchemaObjectHandler
{
  public override bool IsMatch(SchemaObject schema)
  {
    return schema.Type?.ToLower() is "number" or "integer";  
  }

  public override ValueSnippet Construct(SchemaObject schema)
  {
    return new NumberSnippet(schema);
  }


  public NumberHandler(Controller controller) : base(controller)
  {
  }
}