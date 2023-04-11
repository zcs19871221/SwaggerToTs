using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaSnippets;

public class NumberSnippet : ValueSnippet
{

  public NumberSnippet() 
  {
    ExportType = ExportType.Type;
  }

  protected override string GenerateExportedContent(GeneratingInfo generatingInfo)
  {
    return $"export type {ExportName} = {GenerateContent(generatingInfo)};";
  }

  protected override string GenerateContent(GeneratingInfo generatingInfo)
  {
    return "number";
  }
}