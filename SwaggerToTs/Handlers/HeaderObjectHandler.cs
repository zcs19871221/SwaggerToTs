using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.Handlers;

public class HeaderObjectHandler: ReferenceObjectHandler
{
  

  public HeaderObjectHandler(Controller controller) : base(controller)
  {
  }

  public ValueSnippet Generate(HeaderObject headerObject, string key)
  {
    return Handle(headerObject, h =>
    {
      return new KeyValueSnippet(new KeySnippet(key), Controller.ParameterObjectHandler.Generate(h), Controller);
    });
  }


}