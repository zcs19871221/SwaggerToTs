namespace SwaggerToTs.Snippets;

public class KeySnippet:CommonSnippet
{
  
  public string Name { get; set; }
  
  public bool Required { get; set; }
  
  public bool IsReadOnly { get; set; }

  public KeySnippet(string name, bool required = true, bool isReadonly = true)
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

