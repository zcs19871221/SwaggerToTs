using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaSnippets;


public class SchemaSnippet: ValueSnippet
{
  private void _init(SchemaObject schema)
  {
    AddComments(new[]
    {
      (nameof(schema.Title), schema.Title),
      (nameof(schema.Description), schema.Description),
      (nameof(schema.Deprecated), schema.Deprecated ? "true": ""),
      (nameof(schema.Format), schema.Format),
    });
    IsNullable = schema.Nullable;
  }
  
  public static bool IsMatch(SchemaObject schema)
  {
    throw new NotImplementedException();
  }

  public SchemaSnippet(SchemaObject schema)
  {
    _init(schema);
  }
  public static SchemaSnippet GenerateSnippet(SchemaObject schema)
  {
    throw new NotImplementedException();
  }

  public override string GenerateExportedContent(Options options, List<ValueSnippet> imports)
  {
    throw new NotImplementedException();
  }

  public override string GenerateContent(Options options, List<ValueSnippet> imports)
  {
    throw new NotImplementedException();
  }
}