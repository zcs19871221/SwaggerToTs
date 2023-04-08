namespace SwaggerToTs.Snippets;


public class KeyValueSnippets: ValueSnippet
{
  public List<ValueSnippet> Values;
  public KeyValueSnippets(IEnumerable<ValueSnippet> values)
  {
    Values = values.ToList();
  }

  public override string GenerateContent(Options options, List<ValueSnippet> imports)
  {
    return AddBrackets(GetContent(options, imports));
  }

  private string GetContent(Options options, List<ValueSnippet> imports)
  {
    return Values.Aggregate("", (s, snippet) => s + NewLine + snippet.Generate(options, imports));
  }

  public override string GenerateExportedContent(Options options, List<ValueSnippet> imports)
  {
    return CreateComments() + $"export interface {ExportName}" + 
           AddBrackets(GetContent(options, imports));
  }

}

