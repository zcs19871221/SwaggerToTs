using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.Handlers;

public class RequestBodyObjectHandler: ReferenceObjectHandler
{
  
  
  public ValueSnippet? Generate(RequestBodyObject requestBodyObject)
  {

    var content = GetRefOrSelf(requestBodyObject).Content.Where(e =>
    {
      return e.Value.Schema != null;
    });
    var keyValuePairs = content.ToList();
    if (!keyValuePairs.Any())
    {
      return null;
    }
    requestBodyObject.Content = keyValuePairs.ToDictionary(e => e.Key, e=>e.Value);
    return Handle(requestBodyObject, r =>
    {
      var requestBody = new ValuesSnippet(r.Content.Where(e =>
      {
        return e.Value.Schema != null;
      }).Select(e =>
      {
        return new KeyValueSnippet(new KeySnippet(e.Key),
          Controller.SchemaObjectHandlerWrapper.Construct(e.Value.Schema ?? throw new InvalidOperationException()), Controller);
      }));
 
      requestBody.AddComments(new List<(string, string?)>
      {
        (nameof(r.Description), r.Description),
      });
      return requestBody;
    });
  }


  public RequestBodyObjectHandler(Controller controller) : base(controller)
  {
  }
}