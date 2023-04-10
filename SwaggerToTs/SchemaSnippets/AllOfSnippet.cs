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
  private List<ValueSnippet> _allOfs;

  private List<string>? _extends;

  private ValueSnippet? _objectSnippet;


  public AllOfSnippet(SchemaObject schema, Controller controller) : base(schema)
  {
    
    var allof = schema.Allof.Select(controller.SchemaObjectHandlerWrapper.Construct).ToList();
    if (controller.SchemaObjectHandlerWrapper.ObjectHandler.IsMatch(schema))
    {
      allof.Add(controller.SchemaObjectHandlerWrapper.ObjectHandler.Construct(schema));
    }

    CreateAllOfSnippet(allof);
  }
  
  public AllOfSnippet(List<ValueSnippet> allOfs)
  {
    
    CreateAllOfSnippet(allOfs);
  }
  
  private void CreateAllOfSnippet(List<ValueSnippet> allOfs)
  {
    var exportNames = allOfs.Where(e => e is ExportedValueSnippet).Select(e => e.ExportName).ToList();
    var keyValues = allOfs.Where(e => e is KeyValueSnippet or ValuesSnippet).ToList();

    if (exportNames.Count + keyValues.Count == allOfs.Count)
    {
      _extends = exportNames!;
      ExportType = ExportType.Interface;    
    } else
    {
      ExportType = ExportType.Type;
    }

    _allOfs = allOfs;
  }

  public override string GenerateExportedContent(Options options, GeneratingInfo generatingInfo)
  {
    if (ExportType == ExportType.Interface)
    {
      return
        $"export interface {ExportName} {(_extends != null ? " extends" + string.Join(", ", _extends) : "")}{AddBrackets(_objectSnippet == null ? "" : _objectSnippet.Generate(options, generatingInfo))}";
    }
    
    return $"export type {ExportName} = {GenerateContent(options, generatingInfo)}";
  }

  public override string GenerateContent(Options options, GeneratingInfo generatingInfo)
  {

    if (ExportType == ExportType.Interface)
    {
      return string.Join(NewLine, _allOfs.Select(snippet => snippet.Generate(options, generatingInfo)));    
    }

    return string.Join("&", _allOfs.Select(e => e.Generate(options, generatingInfo)));
  }
}