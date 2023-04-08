using SwaggerToTs.Handlers;
using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaSnippets;

public class ObjectSnippet:SchemaSnippet
{

  private KeyValueSnippets propertiesObject;
  public ObjectSnippet(SchemaObject schema, Controller controller) : base(schema)
  {
    propertiesObject = new KeyValueSnippets(schema.Properties.Select(e =>
    {
      return new KeyValueSnippet(new KeySnippet(e.Key), controller.SelectThenConstruct(e.Value),
        controller);
    }).ToList())
    {
      ExportType = ExportType.Interface
    };
    propertiesObject.AddComments(new []
    {
      (nameof(schema.MinProperties), schema.MinProperties.ToString()),
      (nameof(schema.MaxProperties), schema.MaxProperties.ToString())
    });
  }

  public override string GenerateExportedContent(Options options, List<ValueSnippet> imports)
  {
    return $"export interface {ExportName} {AddBrackets(GenerateContent(options, imports))}";
  }

  public override string GenerateContent(Options options, List<ValueSnippet> imports)
  {
    return propertiesObject.Generate(options, imports);
  }
}