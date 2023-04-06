using System.ComponentModel.Design;
using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.SchemaSnippets;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.Handlers;

public class StringHandler: SchemaObjectHandler
{
  public override bool IsMatch(SchemaObject schema)
  {
    return schema.Type == "string";
  }

  public override ValueSnippet Construct(SchemaObject schema)
  {
    return new StringSnippet(schema);
  }


  public StringHandler(Controller controller) : base(controller)
  {
  }
}