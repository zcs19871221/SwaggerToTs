using SwaggerToTs.Handlers;
using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaHandlers;

public class SchemaObjectHandlerWrapper: ReferenceObjectHandler
{


  public List<ISchemaObjectHandler> SchemaHandlers = new();

  public ValueSnippet Construct(SchemaObject schema)
  {
    return Handle(schema, p =>
    {
      var handler = SchemaHandlers.Find(h => h.IsMatch(schema));
      return (handler ?? throw new Exception("cant find handler for schema")).Construct(schema);
    });
  }


  public SchemaObjectHandlerWrapper(Controller controller) : base(controller)
  {
  }
}
  protected SchemaObjectHandler(Controller controller) : base(controller)
  {
  }
}