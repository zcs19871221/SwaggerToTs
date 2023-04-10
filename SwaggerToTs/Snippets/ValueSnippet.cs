
namespace SwaggerToTs.Snippets;

public abstract class ValueSnippet:CommonSnippet
{
  public bool IsNullable { get; set; }
  
  public bool IsReadOnly { get; set; }
  
  public ExportType ExportType { get; set; }
  
  public string? ReferenceUrl { get; set; }

  public string? ExportName { get; set; }
  public string? FileLocate { get; set; }
  public int Priority { get; set; }

  public List<ExportedValueSnippet> UsedBy = new();
  public abstract string GenerateExportedContent(GeneratingInfo generatingInfo);
  public abstract string GenerateContent(GeneratingInfo generatingInfo);

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
    ExportName = exportName;
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

