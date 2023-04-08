using SwaggerToTs.Handlers;
using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaSnippets;

public class ObjectSnippet:SchemaSnippet
{

  private ValuesSnippet propertiesObject;
  public ObjectSnippet(SchemaObject schema, Controller controller) : base(schema)
  {
    propertiesObject = new ValuesSnippet(schema.Properties.Select(e =>
    {
      return new KeyValueSnippet(new KeySnippet(e.Key), controller.SchemaObjectHandlerWrapper.Construct(e.Value),
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

  public override string GenerateExportedContent(Options options, GeneratingInfo generatingInfo)
  {
    return $"export interface {ExportName} {AddBrackets(GenerateContent(options, generatingInfo))}";
  }

  public override string GenerateContent(Options options, GeneratingInfo generatingInfo)
  {
    return propertiesObject.Generate(options, generatingInfo);
  }
}