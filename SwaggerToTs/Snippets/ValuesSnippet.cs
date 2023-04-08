namespace SwaggerToTs.Snippets;


public class KeyValueSnippets: ValueSnippet
{
  public List<ValueSnippet> Values;
  public KeyValueSnippets(IEnumerable<ValueSnippet> values)
  {
    Values = values.ToList();
  }

  public override string GenerateContent(Options options, GeneratingInfo generatingInfo)
  {
    return Values.Aggregate("", (s, snippet) => s + NewLine + snippet.Generate(options, generatingInfo));
  }
  
  public override string GenerateExportedContent(Options options, GeneratingInfo generatingInfo)
  {
    return $"export interface {ExportName}" + 
           AddBrackets(GenerateContent(options, generatingInfo));
  }

}

