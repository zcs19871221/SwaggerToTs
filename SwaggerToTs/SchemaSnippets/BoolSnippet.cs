using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaSnippets;


public class BoolSnippet:ValueSnippet
{
  public override string GenerateExportedContent(Options options, List<ValueSnippet> imports)
  {
    return CreateComments() + $"export type {ExportName} = {GenerateContent(options, imports)};";
  }

  public override string GenerateContent(Options options, List<ValueSnippet> imports)
  {
    return "bool";
  }
}