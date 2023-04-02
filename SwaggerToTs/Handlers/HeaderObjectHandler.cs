using System.ComponentModel.Design;
using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.Handlers;

public class HeaderObjectHandler: ReferenceObjectHandler
{
  

  public HeaderObjectHandler(Controller controller) : base(controller)
  {
  }

  public KeyValueSnippet Generate(HeaderObject headerObject, string key)
  {
    return Handle(headerObject, h =>
    {
      return Controller.ParameterObjectHandler.CreateWrapperSnippet(h, key);
    });
  }


}