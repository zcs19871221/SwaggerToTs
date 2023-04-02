namespace SwaggerToTs.Snippets;

public class ValueSnippet:CommonSnippet
{
  public bool IsNullable { get; set; }
  
  public bool IsReadOnly { get; set; }

  public ExtractedValueSnippet Export()
  {
    
  }
 
  //
  // public string? Generic { get; set; }
  //
  // public override string ToString()
  // {
  //   var content = CreateContent();
  //   if (!string.IsNullOrWhiteSpace(Generic))
  //   {
  //     content = $"{Generic}<{content}>";
  //   }
  //   return IsReadOnly ? "readonly " : "" + content ;
  // }
  //
  // abstract protected string CreateContent();
}


