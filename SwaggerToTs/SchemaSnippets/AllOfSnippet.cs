using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaSnippets;

public enum AllOfGenerateType
{
  Join,
  Interface
}
public class AllOfSnippet : SchemaSnippet
{
  private IEnumerable<ValueSnippet> _allOfs;

  private List<string>? _extends;

  private ValueSnippet? _objectSnippet;

  public AllOfGenerateType Type;
  public AllOfSnippet(SchemaObject schema, Controller controller) : base(schema)
  {
    if (controller.ObjectHandler.IsMatch(schema))
    {
      _objectSnippet = controller.ObjectHandler.Construct(schema);
    }
    _allOfs = schema.Allof.Select(controller.SelectThenConstruct).ToList();

    var exportNames = _allOfs.Where(e => e is ExportedValueSnippet).Select(e => e.ExportName).ToList();
    if (exportNames.Count == _allOfs.Count() && (_objectSnippet == null || _objectSnippet is ExportedValueSnippet))
    {
      _extends = exportNames;
      Type = AllOfGenerateType.Interface;
      ExportType = ExportType.Interface;
    }
    else
    {
      Type = AllOfGenerateType.Join;
      ExportType = ExportType.Type;
    }

  }

  public override string GenerateExportedContent(Options options, List<ValueSnippet> imports)
  {
    if (ExportType == ExportType.Interface)
    {
      return
        $"export interface {ExportName} {(_extends != null ? " extends" + string.Join(", ", _extends) : "")}{AddBrackets(_objectSnippet == null ? "" : _objectSnippet.Generate(options, imports))}";
    }
    
    return $"export type {ExportName} = {GenerateContent(options, imports)}";
  }

  public override string GenerateContent(Options options, List<ValueSnippet> imports)
  {

    if (Type == AllOfGenerateType.Interface)
    {
      throw new Exception("should extract");
    }

    var items = new List<ValueSnippet>();
    items.AddRange(_allOfs);
    if (_objectSnippet != null)
    {
      items.Add(_objectSnippet);
    }
    return $"AllOfSnippet<{string.Join(NewLine, items.Select(e => e.Generate(options, imports)))}>";  }
}