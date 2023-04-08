using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.SchemaSnippets;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaHandlers;

public class EnumHandler: SchemaObjectHandler
{
  public override bool IsMatch(SchemaObject schema)
  {
    return schema.Enum.Any();
  }

  public override ValueSnippet Construct(SchemaObject schema)
  {
    return new EnumSnippet(schema, Controller);
  }


  public EnumHandler(Controller controller) : base(controller)
  {
  }
}