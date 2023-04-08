using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaSnippets;


public class StringSnippet : SchemaSnippet
{

  public override string GenerateExportedContent(Options options, GeneratingInfo generatingInfo)
  {
    return $"export type {ExportName} = ${GenerateContent(options, generatingInfo)};";
  }

  public override string GenerateContent(Options options, GeneratingInfo generatingInfo)
  {
    return "string";
  }

  public StringSnippet(SchemaObject schema) : base(schema)
  {
    ExportType = ExportType.Type;
    AddComments(new []
    {
      (nameof(schema.Pattern), schema.Pattern),
      (nameof(schema.MinLength), schema.MinLength.ToString()),
      (nameof(schema.MaxLength), schema.MaxLength.ToString())
    });
  }
}