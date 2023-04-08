using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaSnippets;

public class OneOfSnippet : SchemaSnippet
{
  private List<ValueSnippet> _oneOfs;

  public OneOfSnippet(SchemaObject schema, Controller controller) : base(schema)
  {
    _oneOfs = schema.Oneof.Select(controller.SchemaObjectHandlerWrapper.Construct).ToList();
  }

  public override string GenerateExportedContent(Options options, GeneratingInfo generatingInfo)
  {
    return $"export type {ExportName} = {GenerateContent(options, generatingInfo)}";
  }

  public override string GenerateContent(Options options, GeneratingInfo generatingInfo)
  {
    generatingInfo.AddHelper(Controller.OneOfName);
    return $"{Controller.OneOfName}<{string.Join(NewLine, _oneOfs.Select(e => e.Generate(options, generatingInfo)))}>";  }
}