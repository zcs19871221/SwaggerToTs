using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaSnippets;


public class StringSnippet : ValueSnippet
{
  protected override string GenerateIsolateContent(GeneratingInfo generatingInfo)
  {
    return $"export type {ExportName} = ${GenerateContent(generatingInfo)};";
  }

  protected override string GenerateContent(GeneratingInfo generatingInfo)
  {
    return "string";
  }

  public StringSnippet() 
  {
    ExportType = ExportType.Type;
  }
}