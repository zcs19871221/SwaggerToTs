using System.ComponentModel.Design;
using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.SchemaSnippets;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.Handlers;

public class ObjectHandler: SchemaObjectHandler
{
  public override bool IsMatch(SchemaObject schema)
  {
    return schema.Type == "object";
  }

  public override ValueSnippet Construct(SchemaObject schema)
  {
    return new ObjectSnippet(schema, Controller);
  }


  public ObjectHandler(Controller controller) : base(controller)
  {
  }
}