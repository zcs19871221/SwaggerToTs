using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.SchemaSnippets;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaHandlers;

public class StringHandler : ISchemaHandler
{
  public bool IsMatch(SchemaObject schema)
  {
    return schema.Type == "string";
  }


  public ValueSnippet GenerateSnippet(SchemaObject schema)
  {

    var snippet = new StringSnippet();
    Helper.IniSchemaSnippet(snippet, schema);
    snippet.AddComments(new []
    {
      (nameof(schema.Pattern), schema.Pattern),
      (nameof(schema.MinLength), schema.MinLength.ToString()),
      (nameof(schema.MaxLength), schema.MaxLength.ToString())
    });
    return snippet;
  }
}