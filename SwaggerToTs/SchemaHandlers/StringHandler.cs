using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.SchemaSnippets;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaHandlers;

public class StringHandler: SchemaObjectHandler
{
  public override bool IsMatch(SchemaObject schema)
  {
    return schema.Type?.ToLower() == "string";
  }

  protected override ValueSnippet DoConstruct(SchemaObject schema)
  {
    var snippet = new StringSnippet();
    snippet.AddComments(new []
    {
      (nameof(schema.Pattern), schema.Pattern),
      (nameof(schema.MinLength), schema.MinLength.ToString()),
      (nameof(schema.MaxLength), schema.MaxLength.ToString())
    });
    return snippet;
  }


  public StringHandler(Controller controller) : base(controller)
  {
  }
}