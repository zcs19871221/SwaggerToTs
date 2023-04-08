using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.SchemaSnippets;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaHandlers;

public class AnyHandler : SchemaObjectHandler
{
  public override bool IsMatch(SchemaObject schema)
  {
    return schema.Type == "object" && !schema.Properties.Any() && !schema.Allof.Any();
  }
  
  public override ValueSnippet Construct(SchemaObject schema)
  {
    return new RecordObjectSnippet(schema);
  }


  public AnyHandler(Controller controller) : base(controller)
  {
  }
}