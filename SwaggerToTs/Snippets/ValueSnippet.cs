namespace SwaggerToTs.Snippets;

public class ValueSnippet:CommonSnippet
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

public class ExtractedValueSnippet:ValueSnippet
{
  private IsolateSnippet _isolateSnippet;

  public ExtractedValueSnippet(IsolateSnippet isolateSnippet)
  {
    IsNullable = false;
    IsReadOnly = false;
    _isolateSnippet = isolateSnippet;
    Dependencies.Add(isolateSnippet);
  }

  public override (List<IsolateSnippet>, string) Generate()
  {
    return (Dependencies, _isolateSnippet.ExportName);
  }
}

