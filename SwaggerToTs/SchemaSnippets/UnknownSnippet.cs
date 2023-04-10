using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaSnippets;

public class UnknownSnippet : ValueSnippet
{
  protected override string GenerateExportedContent(GeneratingInfo generatingInfo)
  {
    throw new Exception("should not export unknown type");
  }

  protected override string GenerateContent(GeneratingInfo generatingInfo)
  {
    return "unknown";
  }
}