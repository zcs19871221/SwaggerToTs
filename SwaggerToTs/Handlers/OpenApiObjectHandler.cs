using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.Handlers;

public class OpenApiObjectHandler : Handler
{
  public Controller Generate(OpenApiObject openApiObject)
  {
    const string name = "Route";
    var routes = 
      new KeyValueSnippets(openApiObject.Paths.Select(p =>
        new KeyValueSnippet(new KeySnippet(p.Key), Controller.PathItemObjectHandler.Generate(p.Key, p.Value))));
    routes.AddComments(new List<(string, string?)>
    {
      (nameof(openApiObject.OpenApi), openApiObject.OpenApi),
      (nameof(openApiObject.Info.Description), openApiObject.Info.Description),
      (nameof(openApiObject.Info.Title), openApiObject.Info.Title),
      (nameof(openApiObject.Info.Version), openApiObject.Info.Version),
    });
    routes.Export(name, name, Controller);
    return Controller;
  }


  public OpenApiObjectHandler(Controller controller) : base(controller)
  {
  }
}