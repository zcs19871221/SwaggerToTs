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

  public override string GenerateExportedContent(Options options, List<ValueSnippet> imports)
  {
    return $"export type {ExportName} = {GenerateContent(options, imports)}";
  }

  public override string GenerateContent(Options options, List<ValueSnippet> imports)
  {
    return @"Record<string, unknown>";  
  }
}