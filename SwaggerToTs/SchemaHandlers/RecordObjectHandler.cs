using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.SchemaSnippets;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaHandlers;

public class RecordObjectHandler : SchemaObjectHandler
{
  public override bool IsMatch(SchemaObject schema)
  {
    return schema.Type == "object" && !schema.Properties.Any() && !schema.Allof.Any();
  }
  
  public override ValueSnippet Construct(SchemaObject schema)
  {
    return new RecordObjectSnippet(schema);
  }


  public RecordObjectHandler(Controller controller) : base(controller)
  {
  }
}