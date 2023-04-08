using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaSnippets;

public class BoolSnippet : SchemaSnippet
{
  public BoolSnippet(SchemaObject schema) : base(schema)
  {
    ExportType = ExportType.Type;
  }

  public override string GenerateExportedContent(Options options, GeneratingInfo generatingInfo)
  {
    return $"export type = {GenerateContent(options, generatingInfo)};";
  }

  public override string GenerateContent(Options options, GeneratingInfo generatingInfo)
  {
    return "boolean";
  }
}