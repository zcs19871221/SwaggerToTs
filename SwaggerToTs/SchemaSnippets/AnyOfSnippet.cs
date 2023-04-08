using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaSnippets;

public class AnyOfSnippet : SchemaSnippet
{
  private List<ValueSnippet> _anyOfs;

  public AnyOfSnippet(SchemaObject schema, Controller controller) : base(schema)
  {
    _anyOfs = schema.AnyOf.Select(controller.SelectThenConstruct).ToList();
  }

  public override string GenerateExportedContent(Options options, GeneratingInfo generatingInfo)
  {
    return $"export type {ExportName} = {GenerateContent(options, generatingInfo)}";
  }

  public override string GenerateContent(Options options, GeneratingInfo generatingInfo)
  {
    generatingInfo.AddHelper(Controller.AnyOfName);
    return $"{Controller.AnyOfName}<{string.Join(NewLine, _anyOfs.Select(e => e.Generate(options, generatingInfo)))}>";  }
}