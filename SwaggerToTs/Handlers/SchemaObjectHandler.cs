using System.ComponentModel.Design;
using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.Handlers;

public class SchemaObjectHandler: ReferenceObjectHandler
{
  

  public SchemaObjectHandler(Controller controller) : base(controller)
  {
  }

  public Snippets.Snippets Generate(SchemaObject schemaObject)
  {
    return Handle(schemaObject, p =>
    {
 
    });
  }


}