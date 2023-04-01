namespace SwaggerToTs.Snippets;

public class IsolateSnippet:CommonSnippet
{

  public IsolateSnippet(string exportName, string fileLocate, ValueSnippet value)
  {
    ExportName = exportName;
    FileLocate = fileLocate;
    Value = value;

  }
  
  public IsolateSnippet(string exportName, string fileLocate, Snippets snippets)
  {
    ExportName = exportName;
    FileLocate = fileLocate;
    Snippets = snippets;
  }

  public int Priority { get; set; }
  public string ExportName { get; set; }
  public string FileLocate { get; set; }
  
  protected ExportType ExportType { get; set; }

  protected HashSet<string> Extends = new();

  public Snippets? Snippets;
  public ValueSnippet? Value;

  public List<ExtractedValueSnippet> UsedBy = new();


  public override (List<IsolateSnippet>, string) Generate()
  {
      if (Value != null)
      {
        var (d, c) = Value.Generate();
        return (Dependencies.Concat(d).ToList(), c);
      }

      var (d1, c1) = Snippets.Generate();

      return (Dependencies.Concat(d1).ToList(), c1);
    }
}


public enum ExportType
{
  Interface,
  Type,
  Enum
}
