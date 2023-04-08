
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
  public abstract string GenerateExportedContent(Options options, GeneratingInfo generatingInfo);
  public abstract string GenerateContent(Options options, GeneratingInfo generatingInfo);

  public string Generate(Options options, GeneratingInfo generatingInfo)
  {
    generatingInfo.AddImports(Dependencies);
    return string.IsNullOrWhiteSpace(ExportName) ? GenerateContent(options, generatingInfo) : CreateComments() + GenerateExportedContent(options, generatingInfo);
  }
  public ExportedValueSnippet Export(string exportName, string fileLocate, Controller controller)
  {
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

