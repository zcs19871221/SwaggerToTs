using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaSnippets;

public class BoolSnippet : ValueSnippet
{
  public BoolSnippet()
  {
    ExportType = ExportType.Type;
  }

  public override string GenerateExportedContent(GeneratingInfo generatingInfo)
  {
    return $"export type = {GenerateContent(generatingInfo)};";
  }

  public override string GenerateContent(GeneratingInfo generatingInfo)
  {
    return "boolean";
  }
}