using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaSnippets;

public class UnknownSnippet : SchemaSnippet
{

  public UnknownSnippet(SchemaObject schema) : base(schema, false)
  {
    
  }

  public override string GenerateExportedContent(Options options, GeneratingInfo generatingInfo)
  {
    return $"export type {ExportName} = {GenerateContent(options, generatingInfo)}";
  }

  public override string GenerateContent(Options options, GeneratingInfo generatingInfo)
  {
    return "unknown";
  }
}