using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.Handlers;

public class RequestBodyObjectHandler: ReferenceObjectHandler
{
  
  
  public KeyValueSnippet? Generate(RequestBodyObject requestBodyObject)
  {
    var element = GetRefOrSelf(requestBodyObject);

    var content = element.Content.Where(e => e.Value.Schema != null).ToDictionary(e => e.Key, e=>e.Value);
    if (!content.Any())
    {
      return null;
    }
    var value = GetOrCreateThenSaveValue(requestBodyObject, r =>
    {
      var requestBody = new KeyValuesSnippet(content.Select(e => new KeyValueSnippet(new KeySnippet(e.Key),
        Controller.SchemaObjectHandlerWrapper.Construct(e.Value.Schema ?? throw new InvalidOperationException()), Controller)));
 
      requestBody.AddComments(new List<(string, string?)>
      {
        (nameof(r.Description), r.Description),
      });
      return requestBody;
    });
    return new KeyValueSnippet(new KeySnippet("Body", element.Required, isFormat:false), value, Controller);
  }


  public RequestBodyObjectHandler(Controller controller) : base(controller)
  {
  }
}