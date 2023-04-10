using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.SchemaSnippets;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaHandlers;

public class RecordObjectHandler : SchemaObjectHandler
{
  public override bool IsMatch(SchemaObject schema)
  {
    return schema.Type?.ToLower() == "object" && !schema.Properties.Any() && !schema.Allof.Any();
  }
  
  public override ValueSnippet DoConstruct(SchemaObject schema)
  {

    var snippet = new RecordObjectSnippet();
    snippet.AddComments(new []
    {
      (nameof(schema.MinProperties), schema.MinProperties.ToString()),
      (nameof(schema.MaxProperties), schema.MaxProperties.ToString())
    });
    return snippet;
  }


  public RecordObjectHandler(Controller controller) : base(controller)
  {
  }
}