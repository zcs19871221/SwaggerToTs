
namespace SwaggerToTs.Snippets;

public abstract class ValueSnippet:CommonSnippet
{
  public bool IsNullable { get; set; }
  
  public bool IsReadOnly { get; set; }
  
  public ExportType ExportType { get; set; }

  public string? ExportName { get; set; }
  public string? FileLocate { get; set; }
  public int Priority { get; set; }

  public List<ExportedValueSnippet> UsedBy = new();
  public abstract string GenerateExportedContent(Options options, List<ValueSnippet> imports);
  public abstract string GenerateContent(Options options, List<ValueSnippet> imports);

  public virtual string Generate(Options options, List<ValueSnippet> imports)
  {
    imports.AddRange(Dependencies);
    return string.IsNullOrWhiteSpace(ExportName) ? GenerateContent(options, imports) : GenerateExportedContent(options, imports);
  }
  public virtual ExportedValueSnippet Export(string exportName, string fileLocate, Controller controller)
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

