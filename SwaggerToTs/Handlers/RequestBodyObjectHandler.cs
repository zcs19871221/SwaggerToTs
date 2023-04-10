using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.Handlers;

public class RequestBodyObjectHandler: ReferenceObjectHandler
{
  
  
  public ValueSnippet Generate(RequestBodyObject requestBodyObject)
  {
    return Handle(requestBodyObject, r =>
    {
      var responseContent = new ValuesSnippet(r.Content.Where(e =>
      {
        return e.Value.Schema != null;
      }).Select(e =>
      {
        return new KeyValueSnippet(new KeySnippet(e.Key),
          Controller.SchemaObjectHandlerWrapper.Construct(e.Value.Schema ?? throw new InvalidOperationException()), Controller);
      }));
      var response = new KeyValueSnippet(new KeySnippet("Body", r.Required, isFormat:false), responseContent, Controller);
      response.AddComments(new List<(string, string?)>
      {
        (nameof(r.Description), r.Description),
      });
      return response;
    });
  }


  public RequestBodyObjectHandler(Controller controller) : base(controller)
  {
  }
}