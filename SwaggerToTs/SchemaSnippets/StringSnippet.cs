using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaSnippets;


public class StringSnippet : ValueSnippet
{

  public override string GenerateExportedContent(GeneratingInfo generatingInfo)
  {
    return $"export type {ExportName} = ${GenerateContent(generatingInfo)};";
  }

  public override string GenerateContent(GeneratingInfo generatingInfo)
  {
    return "string";
  }

  public StringSnippet() 
  {
    ExportType = ExportType.Type;
  }
}