using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.Handlers;

public class ResponseObjectHandler: ReferenceObjectHandler
{
  
  public Snippets.Snippets Generate(ResponseObject responseObject)
  {
    return Handle(responseObject, res =>
    {
      var contents = new List<Snippets.Snippets>();
      if (res.Headers != null)
      {
        var headerContent = new Snippets.Snippets(res.Headers.Select(e =>
        {
          return Controller.HeaderObjectHandler.Generate(e.Value, e.Key);
        }));
        contents.Add(new Snippets.Snippets(new KeySnippet("Headers"), headerContent));
      }

      if (res.Content != null)
      {
        var content = new Snippets.Snippets(res.Content.Where(e => e.Value.Schema != null).Select(e =>
        {
          return new Snippets.Snippets(new KeySnippet(e.Key),
            Controller.SchemaObjectHandler.Generate(e.Value.Schema ?? throw new InvalidOperationException()));
        }));
        contents.Add(new Snippets.Snippets(new KeySnippet("Content"), content));
      }

      var snippet = new Snippets.Snippets(contents);
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