using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaSnippets;


public class UnknownSnippet:CommonSnippet, IValueSnippet
{


  public override string ToString()
  {
    return "unknown";
  }


  public bool IsNullable { get; set; }
  public bool IsReadOnly { get; set; }
}

