using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaSnippets;

public class OneOfSnippet : XOfSnippet
{
  public OneOfSnippet(List<ValueSnippet> oneOfs) : base(oneOfs, Helpers.OneOfName)
  {
  }

}