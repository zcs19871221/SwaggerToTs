namespace SwaggerToTs.Snippets;

public class Value:CommonSnippet
{
  
  protected bool IsNullable { get; set; }
  
  protected bool IsReadOnly { get; set; }

  public override string ToString()
  {
    throw new NotImplementedException();
  }

  public override (List<IsolateSnippet>, string) Generate()
  {
    throw new NotImplementedException();
  }
}

public class ExtractedValue:Value
{
  public IsolateSnippet IsolateSnippet;

  public ExtractedValue(IsolateSnippet isolateSnippet)
  {
    IsNullable = false;
    IsReadOnly = false;
    IsolateSnippet = isolateSnippet;
    Dependencies.Add(isolateSnippet);
  }

  public override (List<IsolateSnippet>, string) Generate()
  {
    return (Dependencies, IsolateSnippet.ExportName);
  }
}

