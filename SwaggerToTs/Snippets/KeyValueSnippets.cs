namespace SwaggerToTs.Snippets;


public class KeyValueSnippets: ValueSnippet
{
  public List<KeyValueSnippet> KeyValues;
  public KeyValueSnippets(List<KeyValueSnippet> keyValues)
  {
    KeyValues = keyValues;
  }

  public override string GenerateExportedContent(Options options, List<ValueSnippet> imports)
  {
    return CreateComments() +
           AddBrackets(GetContent(options, imports));
  }

  private string GetContent(Options options, List<ValueSnippet> imports)
  {
    return KeyValues.Aggregate("", (s, snippet) => s + NewLine + snippet.Generate(options, imports));
  }

  public override string GenerateContent(Options options, List<ValueSnippet> imports)
  {
    return CreateComments() + $"export interface {ExportName}" + 
           AddBrackets(GetContent(options, imports));
  }

}

