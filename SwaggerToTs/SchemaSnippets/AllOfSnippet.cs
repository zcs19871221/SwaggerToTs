using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaSnippets;

public class AllOfSnippet: ValueSnippet
{
  private KeyValuesSnippet _property;

  private List<ExportedValueSnippet> _extends = new ();
  private List<ValueSnippet> _others = new ();

  public AllOfSnippet(List<ValueSnippet> allOfs)
  {
    var keyValues = new List<KeyValueSnippet>();
    foreach (var valueSnippet in allOfs)
    {
      switch (valueSnippet)
      {
        case KeyValueSnippet k:
          keyValues.Add(k);
          break;
        case ExportedValueSnippet e:
          _extends.Add(e);
          break;
        case KeyValuesSnippet kvs:
          keyValues.AddRange(kvs.Values);
          break;
        default:
          _others.Add(valueSnippet);
          break;
      }
    }

    _property = new KeyValuesSnippet(keyValues);
  }
  
  public AllOfSnippet(List<KeyValueSnippet> allOfs)
  {

    _property = new KeyValuesSnippet(allOfs);
  }
  
  public override string GenerateExportedContent(GeneratingInfo generatingInfo)
  {
    if (_others.Count > 0)
    {
      return $"export type {ExportName} = {Join(generatingInfo)}";
    }
  
    var extends = GetExtends(generatingInfo);
    
    return
      $"export interface {ExportName} {(extends.Count > 0 ? $"extends {string.Join(", ", extends)} ": "")}{AddBrackets(_property.Generate(generatingInfo))}";
    
  }

  public override string GenerateContent(GeneratingInfo generatingInfo)
  {
    //
    // if (_property.Count > 0 && (_extends == null || _extends.Count == 0))
    // {
    //   return AddBrackets(string.Join(NewLine, _property.Select(snippet => snippet.Generate(generatingInfo))));    
    // }

    return Join(generatingInfo);
  }
  private List<string> GetExtends(GeneratingInfo generatingInfo)
  {
    if (_extends.Count == 0) return new List<string>();

    var extends = _extends.Select(e => e.Generate(generatingInfo)).ToList();
    extends.Sort();
    return extends;
  }

  private string Join(GeneratingInfo generatingInfo)
  {
    var allOfs = GetExtends(generatingInfo);
    var property = _property.Generate(generatingInfo);
    if (!string.IsNullOrWhiteSpace(property))
    {
      allOfs.Add(AddBrackets(property));
    }
    allOfs.AddRange(_others.Select(e => e.Generate(generatingInfo)).ToList());
    
    return string.Join(" & ", allOfs.Where(e => !string.IsNullOrWhiteSpace(e)));
  }

 
}