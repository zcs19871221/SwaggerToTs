using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaSnippets;
public class XOfSnippet : ValueSnippet
{
  private readonly List<ValueSnippet> _items;

  private readonly string _ofType;
  public XOfSnippet(List<ValueSnippet> items, string ofType)
  {
    _items = items;
    _ofType = ofType;
    ExportType = ExportType.Type;
  }

  protected override string GenerateExportedContent(GeneratingInfo generatingInfo)
  {
    return $"export type {ExportName} = {GenerateContent(generatingInfo)}";
  }

  protected override string GenerateContent(GeneratingInfo generatingInfo)
  {
    generatingInfo.AddHelper(_ofType);
    var content = _items.Select(e =>
    {
      var result = e.Generate(generatingInfo);
      if (e is KeyValueSnippet or KeyValuesSnippet && !string.IsNullOrWhiteSpace(result))
      {
        return AddBrackets(result);
      }

      return result;
    }).Where(e => !string.IsNullOrWhiteSpace(e));
    
    return $"{_ofType}<[{string.Join(", ", content)}]>";  }
}