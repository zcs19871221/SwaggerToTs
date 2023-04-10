using SwaggerToTs.Handlers;
using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaHandlers;

public class SchemaObjectHandlerWrapper: ReferenceObjectHandler
{

  public AllOfHandler AllOfHandler { get; set; }
  public RecordObjectHandler RecordObjectHandler { get; set; }
  public AnyOfHandler AnyOfHandler { get; set; }
  public ArrayHandler ArrayHandler { get; set; }
  public BoolHandler BoolHandler { get; set; }
  public EnumHandler EnumHandler { get; set; }
  
  public NumberHandler NumberHandler { get; set; }
  public ObjectHandler ObjectHandler { get; set; }
  public OneOfHandler OneOfHandler { get; set; }
  
  public StringHandler StringHandler { get; set; }
  public UnknownHandler UnknownHandler { get; set; }

  public List<SchemaObjectHandler> SchemaHandlers;

  public ValueSnippet Construct(SchemaObject schema)
  {
    return GetOrCreateThenSaveValue(schema, p =>
    {
      var handler = SchemaHandlers.Find(h => h.IsMatch(p));
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
    SchemaHandlers = new List<SchemaObjectHandler>()
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