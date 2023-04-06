using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaSnippets;

public class NumberSnippet : SchemaSnippet
{

  public NumberSnippet(SchemaObject schema) : base(schema)
  {
    ExportType = ExportType.Type;
    AddComments(new []
    {
      (nameof(schema.Maximum), schema.Maximum.ToString()),
      (nameof(schema.Maximum), schema.Maximum.ToString()),
      (nameof(schema.ExclusiveMaximum), schema.ExclusiveMaximum.ToString()),
      (nameof(schema.MultipleOf), schema.MultipleOf.ToString())
    });
  }

  public override string GenerateExportedContent(Options options, List<ValueSnippet> imports)
  {
    return CreateComments() + $"export type {ExportName} = {GenerateContent(options, imports)}";
  }

  public override string GenerateContent(Options options, List<ValueSnippet> imports)
  {
    return "number";
  }
}