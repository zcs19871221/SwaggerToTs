namespace SwaggerToTs.Snippets;

public class IsolateSnippet:CommonSnippet
{

  public IsolateSnippet(string exportName, string fileLocate, WrapperSnippet wrapperSnippet)
  {
    ExportName = exportName;
    FileLocate = fileLocate;
    WrapperSnippet = wrapperSnippet;
  }

  public int Priority { get; set; }
  public string ExportName { get; set; }
  public string FileLocate { get; set; }
  
  protected ExportType ExportType { get; set; }

  protected List<string> Extends = new();

  public WrapperSnippet? WrapperSnippet;

  public List<ExtractedValueSnippet> UsedBy = new();


  public override string ToString()
  {
    var content = "";
    if (WrapperSnippet == null)
    {
      throw new Exception(" WrapperSnippet must be not null");
    }
    switch (ExportType)
    {
      case ExportType.Interface:
        var exportName = ExportName;
        if (Extends.Count > 0)
        {
          Extends.Sort((a, b) => string.Compare(a, b, StringComparison.Ordinal));
          exportName += $" extends {string.Join(", ", Extends)}";
        }

        return $"export interface {exportName} {content}";
      case ExportType.Type:
        return $"export type {ExportName} = {content};";
      case ExportType.Enum:
        return $"export enum {ExportName} {content}";
      default:
        throw new Exception("To be exported code doesn't  have export type");
    }

  }  

 
}


public enum ExportType
{
  Interface,
  Type,
  Enum
}
