namespace SwaggerToTs.Snippets;

public class Isolate:CommonSnippet
{

  public Isolate(string exportName, string fileLocate, Value value)
  {
    ExportName = exportName;
    FileLocate = fileLocate;
    Value = value;

  }
  
  public Isolate(string exportName, string fileLocate, Wrapper wrapper)
  {
    ExportName = exportName;
    FileLocate = fileLocate;
    Wrapper = wrapper;
  }

  public int Priority { get; set; }
  public string ExportName { get; set; }
  public string FileLocate { get; set; }
  
  protected ExportType ExportType { get; set; }

  protected List<string> Extends = new();

  public Wrapper? Wrapper;
  public Value? Value;

  public List<ExtractedValue> UsedBy = new();


  public override string ToString()
  {
    var content = "";
    if (Value != null)
    {
      content = Value.ToString();
    } else if (Wrapper != null)
    {
      content = Wrapper.ToString();
    }
    else
    {
      throw new Exception("One of Value and Wrapper must be not null");
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
