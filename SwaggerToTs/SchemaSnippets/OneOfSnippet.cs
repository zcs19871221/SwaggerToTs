using SwaggerToTs.Handlers;
using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaSnippets;

public class OneOfSnippet : SchemaSnippet
{
  private IEnumerable<ValueSnippet> _oneOfs;

  public OneOfSnippet(SchemaObject schema, Controller controller) : base(schema)
  {
    _oneOfs = schema.Oneof.Select(controller.SchemaObjectHandler.SelectThenConstruct);

    HelperNames.Add("OneOf");
  }

  public override string GenerateExportedContent(Options options, List<ValueSnippet> imports)
  {
    return $"export type {ExportName} = {GenerateContent(options, imports)}";
  }

  public override string GenerateContent(Options options, List<ValueSnippet> imports)
  {
    return $"OneOf<{string.Join(NewLine, _oneOfs.Select(e => e.Generate(options, imports)))}>";  }
}