using System.ComponentModel.Design;
using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.SchemaSnippets;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.Handlers;

public class EnumHandler: SchemaObjectHandler
{
  public override bool IsMatch(SchemaObject schema)
  {
    return schema.Enum.Any();
  }

  public override ValueSnippet Construct(SchemaObject schema)
  {
    return new EnumSnippet(schema, Controller.Options);
  }


  public EnumHandler(Controller controller) : base(controller)
  {
  }
}