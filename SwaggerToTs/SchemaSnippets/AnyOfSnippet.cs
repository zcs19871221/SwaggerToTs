using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaSnippets;

public class AnyOfSnippet : SchemaSnippet
{
  private List<ValueSnippet> _anyOfs;

  public AnyOfSnippet(SchemaObject schema, Controller controller) : base(schema)
  {
    _anyOfs = schema.AnyOf.Select(controller.SelectThenConstruct).ToList();

    HelperNames.Add("AnyOf");
  }

  public override string GenerateExportedContent(Options options, List<ValueSnippet> imports)
  {
    return $"export type {ExportName} = {GenerateContent(options, imports)}";
  }

  public override string GenerateContent(Options options, List<ValueSnippet> imports)
  {
    return $"AnyOf<{string.Join(NewLine, _anyOfs.Select(e => e.Generate(options, imports)))}>";  }
}