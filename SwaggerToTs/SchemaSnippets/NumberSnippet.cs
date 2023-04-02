using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaSnippets;


public class NumberSnippet:CommonSnippet, IValueSnippet
{
  

  public override string ToString()
  {
    return "number";
  }


  public bool IsNullable { get; set; }
  public bool IsReadOnly { get; set; }
}

