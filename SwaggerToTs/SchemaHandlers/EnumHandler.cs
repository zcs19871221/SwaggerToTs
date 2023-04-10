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

  protected override ValueSnippet DoConstruct(SchemaObject schema)
  {
    var enums = schema.Enum;
    var exportType = ExportType.Type;
    if (Controller.Options.Get<EnumUseEnum>().Value)
    {
      exportType = ExportType.Enum;
    }
    return new EnumSnippet(enums, exportType);
  }


  public EnumHandler(Controller controller) : base(controller)
  {
  }
}