using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaSnippets;

public class UnknownSnippet : ValueSnippet
{


  public override string GenerateExportedContent(GeneratingInfo generatingInfo)
  {
    throw new Exception("should not export unknown type");
  }

  public override string GenerateContent(GeneratingInfo generatingInfo)
  {
    return "unknown";
  }
}