using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaSnippets;

public class NumberSnippet : ValueSnippet
{

  public NumberSnippet(SchemaObject schema) 
  {
    ExportType = ExportType.Type;
  }

  public override string GenerateExportedContent(GeneratingInfo generatingInfo)
  {
    return $"export type {ExportName} = {GenerateContent(generatingInfo)}";
  }

  public override string GenerateContent(GeneratingInfo generatingInfo)
  {
    return "number";
  }
}