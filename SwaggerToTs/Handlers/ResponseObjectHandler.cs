using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.Handlers;

public class ResponseObjectHandler: ReferenceObjectHandler
{
  
  public ValueSnippet Generate(ResponseObject responseObject)
  {
    return Handle(responseObject, res =>
    {
      var contents = new List<KeyValueSnippet>();
      if (res.Headers != null)
      {
        var headerContent = new ValuesSnippet(res.Headers.Select(e =>
        {
          return Controller.HeaderObjectHandler.Generate(e.Value, e.Key);
        }));
        contents.Add(new KeyValueSnippet(new KeySnippet("Headers"), headerContent, Controller));
      }

      if (res.Content != null)
      {
        var content = new ValuesSnippet(res.Content.Where(e => e.Value.Schema != null).Select(e =>
        {
          return new KeyValueSnippet(new KeySnippet(e.Key),
            Controller.SchemaObjectHandlerWrapper.Construct(e.Value.Schema ?? throw new InvalidOperationException()), Controller);
        }));
        contents.Add(new KeyValueSnippet(new KeySnippet("Content", isFormat:false), content, Controller));
      }
      else
      {
        contents.Add(new KeyValueSnippet(new KeySnippet("Content",isFormat:false), new NullSnippet(), Controller));
      }

      var snippet = new ValuesSnippet(contents);
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