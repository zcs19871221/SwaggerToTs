using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.SchemaSnippets;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaHandlers;

public class ObjectHandler: SchemaObjectHandler
{
  public override bool IsMatch(SchemaObject schema)
  {
    return schema.Type?.ToLower() == "object";
  }

  protected override ValueSnippet DoConstruct(SchemaObject schema)
  {
    var snippet = new KeyValuesSnippet(schema.Properties.Select(e =>
    {
      var isRequired = schema.Required.Contains(e.Key);
      return new KeyValueSnippet(new KeySnippet(e.Key, required:isRequired, isReadonly: true),
        Controller.SchemaObjectHandlerWrapper.Construct(e.Value),
        Controller);
    }).ToList());
    snippet.AddComments(new []
    {
      (nameof(schema.MinProperties), schema.MinProperties.ToString()),
      (nameof(schema.MaxProperties), schema.MaxProperties.ToString())
    });
    return snippet;
  }


  public ObjectHandler(Controller controller) : base(controller)
  {
  }
}