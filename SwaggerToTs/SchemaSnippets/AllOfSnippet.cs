using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaSnippets;

public class AllOfSnippet: ValueSnippet
{
  private List<ValueSnippet> _properties;

  private List<ExportedValueSnippet>? _extends;

  public AllOfSnippet(List<ValueSnippet> properties, List<ExportedValueSnippet>? extends)
  {

    _properties = ValuesSnippet.Sort(properties);
    _extends = extends;
  }
  
  public override string GenerateExportedContent(GeneratingInfo generatingInfo)
  {
    if (_properties.Count == 0)
    {
      return $"export type {ExportName} = {Join(generatingInfo)}";
    }
  
    var extends = GetExtends(generatingInfo);
    
    return
      $"export interface {ExportName} {(extends.Count > 0 ? $"extends {string.Join(", ", extends)} ": "")}{AddBrackets(string.Join(NewLine, _properties.Select(snippet => snippet.Generate(generatingInfo))))}";
    
    // return $"export type {ExportName} = {Join(generatingInfo)}";
  }

  private List<string> GetExtends(GeneratingInfo generatingInfo)
  {
    if (!(_extends?.Count > 0)) return new List<string>();

    var extends = _extends.Select(e => e.Generate(generatingInfo)).ToList();
    extends.Sort();
    return extends;
  }
  public string Join(GeneratingInfo generatingInfo)
  {
    var extends = GetExtends(generatingInfo);
    return string.Join(" & ", extends.Concat(_properties.Select(e => AddBrackets(e.Generate(generatingInfo)))));
  }

  public override string GenerateContent(GeneratingInfo generatingInfo)
  {

    if (_properties.Count > 0 && (_extends == null || _extends.Count == 0))
    {
      return AddBrackets(string.Join(NewLine, _properties.Select(snippet => snippet.Generate(generatingInfo))));    
    }

    return Join(generatingInfo);
  }
}