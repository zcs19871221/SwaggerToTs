using System.ComponentModel.Design;
using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.Handlers;

public abstract class SchemaObjectHandler: ReferenceObjectHandler
{
  
  abstract public bool IsMatch(SchemaObject schema);
  abstract public ValueSnippet DoGenerate(SchemaObject schema);

  public SchemaObjectHandler(Controller controller) : base(controller)
  {
  }

  public ValueSnippet Generate(SchemaObject schemaObject)
  {
    return Handle(schemaObject, p =>
    {
      return WrapperSnippet.Create(DoGenerate(p));
    });
  }


}