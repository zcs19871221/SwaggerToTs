using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.Handlers;

public class ResponseObjectHandler: ReferenceObjectHandler
{
  
  public KeyValueSnippet Generate(ResponseObject responseObject)
  {
    return Handle(responseObject, res =>
    {
      var contents = new List<KeyValueSnippet>();
      if (res.Headers != null)
      {
        var headerContent = KeyValueSnippet.Create(res.Headers.Select(e =>
        {
          return Controller.HeaderObjectHandler.Generate(e.Value, e.Key);
        }));
        contents.Add(KeyValueSnippet.Create(new KeySnippet("Headers"), headerContent));
      }

      if (res.Content != null)
      {
        var content =KeyValueSnippet.Create(res.Content.Where(e => e.Value.Schema != null).Select(e =>
        {
          return KeyValueSnippet.Create(new KeySnippet(e.Key),
            Controller.SchemaObjectHandler.Generate(e.Value.Schema ?? throw new InvalidOperationException()));
        }));
        contents.Add(KeyValueSnippet.Create(new KeySnippet("Content"), content));
      }

      var snippet = KeyValueSnippet.Create(contents);
      snippet.AddComments(new List<(string, string?)>
      {
        (nameof(res.Description), res.Description)
      });
      return snippet;
    });
  }


  public ResponseObjectHandler(Controller controller) : base(controller)
  {
  }
}