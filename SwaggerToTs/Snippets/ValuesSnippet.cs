namespace SwaggerToTs.Snippets;


public class ValuesSnippet: ValueSnippet
{
  public List<ValueSnippet> Values;
  public ValuesSnippet(IEnumerable<ValueSnippet> values)
  {
    Values = Sort(values);
  }

  public static List<ValueSnippet> Sort(IEnumerable<ValueSnippet> values)
  {
    var result = values.ToList();
    result.Sort((a, b) =>
    {
      if (a is KeyValueSnippet aa && b is KeyValueSnippet bb)
      {
        return string.CompareOrdinal(aa.Key.Name, bb.Key.Name);
      }

      return 0;
    });
    return result;
  }
  public override string GenerateContent(GeneratingInfo generatingInfo)
  {
    return string.Join(NewLine, Values.Select(snippet => snippet.Generate(generatingInfo)));
  }
  
  public override string GenerateExportedContent(GeneratingInfo generatingInfo)
  {
    return $"export interface {ExportName} " + 
           AddBrackets(GenerateContent(generatingInfo));
  }

}

