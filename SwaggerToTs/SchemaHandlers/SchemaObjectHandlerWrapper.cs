using SwaggerToTs.Handlers;
using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaHandlers;

public class SchemaObjectHandlerWrapper: ReferenceObjectHandler
{
  private AllOfHandler AllOfHandler { get; }
  private RecordObjectHandler RecordObjectHandler { get; }
  private AnyOfHandler AnyOfHandler { get;  }
  private ArrayHandler ArrayHandler { get;  }
  private BoolHandler BoolHandler { get; }
  private EnumHandler EnumHandler { get; }
  
  private NumberHandler NumberHandler { get; }
  public ObjectHandler ObjectHandler { get;  }
  private OneOfHandler OneOfHandler { get; }
  
  private StringHandler StringHandler { get; }
  private UnknownHandler UnknownHandler { get;  }

  private readonly List<SchemaObjectHandler> _schemaHandlers;

  public ValueSnippet Construct(SchemaObject schema)
  {
    return GetOrCreateThenSaveValue(schema, p =>
    {
      var handler = _schemaHandlers.Find(h => h.IsMatch(p));
      return (handler ?? throw new Exception("cant find handler for schema")).Construct(p);
    });
  }


  public SchemaObjectHandlerWrapper(Controller controller) : base(controller)
  {
    AllOfHandler = new AllOfHandler(controller);
    RecordObjectHandler = new RecordObjectHandler(controller);
    AnyOfHandler = new AnyOfHandler(controller);
    ArrayHandler = new ArrayHandler(controller);
    BoolHandler = new BoolHandler(controller);
    EnumHandler = new EnumHandler(controller);
    NumberHandler = new NumberHandler(controller);
    ObjectHandler = new ObjectHandler(controller);
    OneOfHandler = new OneOfHandler(controller);
    UnknownHandler = new UnknownHandler(controller);
    StringHandler = new StringHandler(controller);
    _schemaHandlers = new List<SchemaObjectHandler>()
    {
      EnumHandler,
      OneOfHandler,
      AnyOfHandler,
      AllOfHandler,
      StringHandler,
      NumberHandler,
      BoolHandler,
      ArrayHandler,
      RecordObjectHandler,
      ObjectHandler,
      UnknownHandler,
    };
  }
}