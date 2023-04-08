using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaSnippets;


public class StringSnippet : SchemaSnippet
{

  public override string GenerateExportedContent(Options options, List<ValueSnippet> imports)
  {
    return $"export type {ExportName} = ${GenerateContent(options, imports)};";
  }

  public override string GenerateContent(Options options, List<ValueSnippet> imports)
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