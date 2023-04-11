
using SwaggerToTs.Handlers;

namespace SwaggerToTs.Snippets;

public abstract class ValueSnippet:CommonSnippet
{
  public bool IsNullable { get; set; }
  
  public bool IsReadOnly { get; protected init; }
  
  public ExportType ExportType { get; protected init; }
  
  public string? ExportName { get; private set; }
  public string? FileLocate { get; private set; }
  public int Priority { get; set; }
  protected abstract string GenerateExportedContent(GeneratingInfo generatingInfo);
  protected abstract string GenerateContent(GeneratingInfo generatingInfo);

  public string Generate(GeneratingInfo generatingInfo)
  {
    generatingInfo.AddImports(Dependencies);
    return string.IsNullOrWhiteSpace(ExportName) ? GenerateContent(generatingInfo) : CreateComments() + GenerateExportedContent(generatingInfo);
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

public enum ExportType
{
  Interface,
  Type,
  Enum
}

