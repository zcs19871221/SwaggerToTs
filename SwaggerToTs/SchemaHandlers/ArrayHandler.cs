using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.SchemaSnippets;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaHandlers;

public class ArrayHandler : SchemaObjectHandler
{
  public override bool IsMatch(SchemaObject schema)
  {
    return schema.Type?.ToLower() == "array";
  }

  public override ValueSnippet DoConstruct(SchemaObject schema)
  {
    var item = Controller.SchemaObjectHandlerWrapper.Construct(schema.Items ??
                                                                       throw new InvalidOperationException());
    var arraySnippet = new ArraySnippet(item);
    arraySnippet.AddComments(new []
    {
      (nameof(schema.MaxItems), schema.MaxItems.ToString()),
      (nameof(schema.MinItems), schema.MinItems.ToString()),
      (nameof(schema.UniqueItems), schema.UniqueItems.ToString())
    });
    return arraySnippet;
  }

  public ArrayHandler(Controller controller) : base(controller)
  {
  }
}