using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.Handlers;

public class ResponseObjectHandler: ReferenceObjectHandler
{
  
  public KeyValueSnippet Generate(Dictionary<string,ResponseObject> responses)
  {
    
    var responseContent =
      new List<KeyValueSnippet>(responses
        .Select(e => new KeyValueSnippet(new KeySnippet(e.Key, isFormat:false), GenerateResponseItem(e.Value), Controller)));
    
    var response = new KeyValueSnippet(new KeySnippet("Responses", isFormat:false),new KeyValuesSnippet(responseContent), Controller);

    return response;
  }

  private ValueSnippet GenerateResponseItem(ResponseObject responseObject)
  {
    return GetOrCreateThenSaveValue(responseObject, res =>
    {
      var contents = new List<KeyValueSnippet>();
      if (res.Headers != null)
      {
        var headerContent = new KeyValuesSnippet(res.Headers.Select(e => Controller.HeaderObjectHandler.Generate(e.Value, e.Key)));
        contents.Add(new KeyValueSnippet(new KeySnippet("Headers", isFormat: false), headerContent, Controller));
      }

      if (res.Content != null)
      {
        var content = new KeyValuesSnippet(res.Content.Where(e => e.Value.Schema != null).Select(e => new KeyValueSnippet(new KeySnippet(e.Key),
          Controller.SchemaObjectHandlerWrapper.Construct(e.Value.Schema ?? throw new InvalidOperationException()),
          Controller)));
        contents.Add(new KeyValueSnippet(new KeySnippet("Content", isFormat: false), content, Controller));
      }
      else
      {
        contents.Add(new KeyValueSnippet(new KeySnippet("Content", isFormat: false), new NullSnippet(), Controller));
      }

      var snippet = new KeyValuesSnippet(contents);
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