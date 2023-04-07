using SwaggerToTs.Handlers;
using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaHandlers;

abstract public class SchemaObjectHandler: ReferenceObjectHandler
{

  private static List<SchemaObjectHandler> _handlers = new List<SchemaObjectHandler>();
  public abstract bool IsMatch(SchemaObject schema);

  public abstract ValueSnippet Construct(SchemaObject schema);

  public ValueSnippet SelectThenConstruct(SchemaObject schema)
  {
    var handler = _handlers.Find(h => h.IsMatch(schema));
    return (handler ?? throw new Exception("fff")).Construct(schema);
  }
  public SchemaObjectHandler(Controller controller) : base(controller)
  {
  }
}