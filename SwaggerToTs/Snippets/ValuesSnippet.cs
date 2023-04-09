namespace SwaggerToTs.Snippets;


public class ValuesSnippet: ValueSnippet
{
  public List<ValueSnippet> Values;
  public ValuesSnippet(IEnumerable<ValueSnippet> values)
  {
    Values = values.ToList();
    Values.Sort((a, b) =>
    {
      if (a is KeyValueSnippet aa && b is KeyValueSnippet bb)
      {
        return string.CompareOrdinal(aa.Key.Name, bb.Key.Name);
      }

      return 0;
    });
  }

  public override string GenerateContent(Options options, GeneratingInfo generatingInfo)
  {
    return string.Join(NewLine, Values.Select(snippet => snippet.Generate(options, generatingInfo)));
  }
  
  public override string GenerateExportedContent(Options options, GeneratingInfo generatingInfo)
  {
    return $"export interface {ExportName} " + 
           AddBrackets(GenerateContent(options, generatingInfo));
  }

}

