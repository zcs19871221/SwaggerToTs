
using SwaggerToTs.Handlers;

namespace SwaggerToTs.Snippets;

public abstract class ValueSnippet:IsolatedSnippet
{
  public bool IsNullable { get; set; }
  public bool IsReadOnly { get; protected init; }
  public int Priority { get; set; }

  protected abstract string GenerateContent(GeneratingInfo generatingInfo);

  public string Generate(GeneratingInfo generatingInfo)
  {
    generatingInfo.AddImports(Dependencies);
    return IsIsolateSnippet() ? GenerateIsolate(generatingInfo) : GenerateContent(generatingInfo);
  }
  
  public ExportedValueSnippet Export(string exportName, string fileLocate, Controller controller)
  {
    if (this is ExportedValueSnippet alreadyExport)
    {
      return alreadyExport;
    }
    ExportName = Handler.ToPascalCase(exportName);
    FileLocate = fileLocate;
    var extractedSnippet = new ExportedValueSnippet(this, controller);
    return extractedSnippet;    
  }
}


