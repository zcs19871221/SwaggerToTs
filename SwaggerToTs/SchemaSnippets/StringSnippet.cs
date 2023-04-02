using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaSnippets;


public class StringSnippet : SchemaSnippet
{
  public new static bool IsMatch(SchemaObject schema)
  {
    return schema.Type == "string";
  }

  
  public new static StringSnippet GenerateSnippet(SchemaObject schema)
  {

    var snippet = new StringSnippet(schema);
    snippet.ExportType = ExportType.Type;
    snippet.AddComments(new []
    {
      (nameof(schema.Pattern), schema.Pattern),
      (nameof(schema.MinLength), schema.MinLength.ToString()),
      (nameof(schema.MaxLength), schema.MaxLength.ToString())
    });
    return snippet;
  }

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
  }
}