namespace SwaggerToTs.Snippets;


public class KeyValuesSnippet: ValueSnippet
{
  public readonly List<KeyValueSnippet> Values;
  public KeyValuesSnippet(IEnumerable<KeyValueSnippet> values)
  {
    Values = values.ToList();
    Values.Sort((a,b) => string.CompareOrdinal(a.Key.Name, b.Key.Name));
  }

  protected override string GenerateContent(GeneratingInfo generatingInfo)
  {
    return string.Join(NewLine, Values.Select(snippet => snippet.Generate(generatingInfo)));
  }

  protected override string GenerateExportedContent(GeneratingInfo generatingInfo)
  {
    return $"export interface {ExportName} " + 
           AddBrackets(GenerateContent(generatingInfo));
  }

}

