using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaSnippets;

public class BoolSnippet : ValueSnippet
{
  public BoolSnippet()
  {
    ExportType = ExportType.Type;
  }

  protected override string GenerateExportedContent(GeneratingInfo generatingInfo)
  {
    return $"export type = {GenerateContent(generatingInfo)};";
  }

  protected override string GenerateContent(GeneratingInfo generatingInfo)
  {
    return "boolean";
  }
}