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
    var value = GetOrCreateThenSaveValue(headerObject, h => Controller.ParameterObjectHandler.Generate(h, true));
    return new KeyValueSnippet(new KeySnippet(key, GetRefOrSelf(headerObject).Required),value , Controller);
  }


}