namespace SwaggerToTs.Snippets;

public class ValueSnippet:CommonSnippet
{
  protected bool IsNullable { get; set; }
  
  protected bool IsReadOnly { get; set; }

}

public class ValuesSnippet:CommonSnippet
{
  
  protected bool IsNullable { get; set; }
  
  protected bool IsReadOnly { get; set; }

}


public class ExtractedValueSnippet:ValueSnippet
{
  public IsolateSnippet IsolateSnippet;

  public ExtractedValueSnippet(IsolateSnippet isolateSnippet)
  {
    IsNullable = false;
    IsReadOnly = false;
    IsolateSnippet = isolateSnippet;
    Dependencies.Add(isolateSnippet);
  }

  public override string ToString()
  {
    return IsolateSnippet.ExportName;
  }

}

