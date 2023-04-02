using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.Handlers;

public class RequestBodyObjectHandler: ReferenceObjectHandler
{
  
  
  public KeyValueSnippet Generate(RequestBodyObject requestBodyObject)
  {
    return Handle(requestBodyObject, r =>
    {
      var snippetContent = KeyValueSnippet.Create(r.Content.Where(e =>
      {
        return e.Value.Schema != null;
      }).Select(e =>
      {
        return KeyValueSnippet.Create(new KeySnippet(e.Key),
          Controller.SchemaObjectHandler.Generate(e.Value.Schema ?? throw new InvalidOperationException()));
      }));
      var snippet = KeyValueSnippet.Create(new KeySnippet("Request", r.Required), snippetContent);
      snippet.AddComments(new List<(string, string?)>
      {
        (nameof(r.Description), r.Description),
      });
      return snippet;
    });
  }


  public RequestBodyObjectHandler(Controller controller) : base(controller)
  {
  }
}