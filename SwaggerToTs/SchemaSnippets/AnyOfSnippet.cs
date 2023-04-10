using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaSnippets;

public class AnyOfSnippet : XOfSnippet
{

  public AnyOfSnippet(List<ValueSnippet> anyOfs): base(anyOfs, Controller.AnyOfName)
  {
  }
}