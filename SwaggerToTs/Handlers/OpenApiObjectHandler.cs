using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.Handlers;

public class OpenApiObjectHandler : Handler
{
  public void Construct(OpenApiObject openApiObject)
  {
    const string name = "Route";
    var routes =
      new KeyValueSnippets(openApiObject.Paths.Select(p =>
        new KeyValueSnippet(new KeySnippet(p.Key), Controller.PathItemObjectHandler.Generate(p.Key, p.Value),
          Controller)));
    routes.AddComments(new List<(string, string?)>
    {
      (nameof(openApiObject.OpenApi), openApiObject.OpenApi),
      (nameof(openApiObject.Info.Description), openApiObject.Info.Description),
      (nameof(openApiObject.Info.Title), openApiObject.Info.Title),
      (nameof(openApiObject.Info.Version), openApiObject.Info.Version),
    });
    routes.Export(name, name, Controller);
  }


  public OpenApiObjectHandler(Controller controller) : base(controller)
  {
  }
  
}