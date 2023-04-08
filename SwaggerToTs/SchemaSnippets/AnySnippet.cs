using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaSnippets;

public class AnySnippet : SchemaSnippet
{
  public AnySnippet(SchemaObject schema) : base(schema)
  {
    AddComments(new []
    {
      (nameof(schema.MinProperties), schema.MinProperties.ToString()),
      (nameof(schema.MaxProperties), schema.MaxProperties.ToString())
    });
    ExportType = ExportType.Type;
  }

  public override string GenerateExportedContent(Options options, GeneratingInfo generatingInfo)
  {
    return $"export type {ExportName} = {GenerateContent(options, generatingInfo)}";
  }

  public override string GenerateContent(Options options, GeneratingInfo generatingInfo)
  {
    return @"Record<string, unknown>";  
  }
}