using System.ComponentModel.Design;
using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.SchemaSnippets;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.Handlers;

public class BoolHandler: SchemaObjectHandler
{
  public override bool IsMatch(SchemaObject schema)
  {
    return schema.Type == "boolean";
  }

  public override ValueSnippet Construct(SchemaObject schema)
  {
    return new BoolSnippet(schema);
  }


  public BoolHandler(Controller controller) : base(controller)
  {
  }
}