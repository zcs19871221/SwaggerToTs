using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaHandlers;

public abstract class SchemaObjectHandler
{
  protected Controller Controller;
  abstract public bool IsMatch(SchemaObject schema);

  abstract public ValueSnippet Construct(SchemaObject schema);

  public SchemaObjectHandler(Controller controller)
  {
    Controller = controller;
  }
}