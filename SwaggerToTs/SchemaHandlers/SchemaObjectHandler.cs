using SwaggerToTs.Handlers;
using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaHandlers;

public abstract class SchemaObjectHandler: ReferenceObjectHandler
{
  public abstract bool IsMatch(SchemaObject schema);

  public abstract ValueSnippet Construct(SchemaObject schema);

  protected SchemaObjectHandler(Controller controller) : base(controller)
  {
  }
}