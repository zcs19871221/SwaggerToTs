using System.Text.RegularExpressions;
using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaSnippets;


public class StringSnippet:ValueSnippet
{
  public override string GenerateExportedContent(Options options, List<ValueSnippet> imports)
  {
    return CreateComments() + $"export type {ExportName} = {GenerateContent(options, imports)};";
  }

  public override string GenerateContent(Options options, List<ValueSnippet> imports)
  {
    return "string";
  }

}

