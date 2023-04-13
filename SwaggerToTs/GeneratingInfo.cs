using SwaggerToTs.Snippets;

namespace SwaggerToTs;


public class GeneratingInfo
{
  private readonly HashSet<ValueSnippet> _imports = new();
  public readonly HashSet<string> HelperNames = new();
  public readonly Controller Controller;

  public GeneratingInfo(Controller controller)
  {
    Controller = controller;
  }

  public IsolatedSnippet? InWhichIsolateSnippet { get; set; }


  public void AddImports(List<ValueSnippet> imports)
  {
    foreach (var import in imports)
    {
      _imports.Add(import);
    }
  }
  
  public void AddHelper(string helperName)
  {
    HelperNames.Add(helperName);
  }
  
  public void AggregateImports(SortedDictionary<string, HashSet<string>> fileMappingExportNames, string fileLocate)
  {
    
    foreach (var codeDependency in _imports)
    {
      if (codeDependency.ExportName == null || string.IsNullOrEmpty(codeDependency.FileLocate)) throw new Exception("exportName or fileLocate should not have empty valueSnippet");

      if (codeDependency.FileLocate != fileLocate) fileMappingExportNames.GetOrCreate(codeDependency.FileLocate).Add(codeDependency.ExportName);
    }


    foreach (var helperName in HelperNames)
    {      
      fileMappingExportNames.GetOrCreate(Helpers.FileName).Add(helperName);
    }
  }
}
