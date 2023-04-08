using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaSnippets;

public class BoolSnippet : SchemaSnippet
{
  public BoolSnippet(SchemaObject schema) : base(schema)
  {
    ExportType = ExportType.Type;
  }

  public override string GenerateExportedContent(Options options, List<ValueSnippet> imports)
  {
    return $"export type = {GenerateContent(options, imports)};";
  }

  public override string GenerateContent(Options options, List<ValueSnippet> imports)
  {
    return "boolean";
  }
}