namespace SwaggerToTs.Snippets;

public class KeySnippet:CommonSnippet
{
  
  protected string Name { get; set; }
  
  protected bool Required { get; set; }
  
  protected bool IsReadOnly { get; set; }

  public KeySnippet(string name, bool required = false, bool isReadonly = true)
  {
    Name = name;
    Required = required;
    IsReadOnly = isReadonly;
  }

  public override string ToString()
  {
    return $"{(IsReadOnly ? "readonly " : "")}{Name}{(Required ? "" : "?")}:";
  }
}

