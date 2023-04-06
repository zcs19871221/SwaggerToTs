using System.ComponentModel.Design;
using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.SchemaSnippets;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.Handlers;

public class UnknownHandler: SchemaObjectHandler
{
  public override bool IsMatch(SchemaObject schema)
  {
    return true;
  }

  public override ValueSnippet Construct(SchemaObject schema)
  {
    return new UnknownSnippet(schema);
  }


  public UnknownHandler(Controller controller) : base(controller)
  {
  }
}