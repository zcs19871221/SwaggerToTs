using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.Handlers;

public class ResponseObjectHandler: ReferenceObjectHandler
{
  
  public WrapperSnippet Generate(ResponseObject responseObject)
  {
    return Handle(responseObject, res =>
    {
      var contents = new List<WrapperSnippet>();
      if (res.Headers != null)
      {
        var headerContent = WrapperSnippet.Create(res.Headers.Select(e =>
        {
          return Controller.HeaderObjectHandler.Generate(e.Value, e.Key);
        }));
        contents.Add(WrapperSnippet.Create(new KeySnippet("Headers"), headerContent));
      }

      if (res.Content != null)
      {
        var content =WrapperSnippet.Create(res.Content.Where(e => e.Value.Schema != null).Select(e =>
        {
          return WrapperSnippet.Create(new KeySnippet(e.Key),
            Controller.SchemaObjectHandler.Generate(e.Value.Schema ?? throw new InvalidOperationException()));
        }));
        contents.Add(WrapperSnippet.Create(new KeySnippet("Content"), content));
      }

      var snippet = WrapperSnippet.Create(contents);
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