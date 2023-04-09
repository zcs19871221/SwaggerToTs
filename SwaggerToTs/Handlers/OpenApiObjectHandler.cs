using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.Handlers;

public class OpenApiObjectHandler : Handler
{
  public void Construct(OpenApiObject openApiObject)
  {
    const string name = "Routes";
    var routes =
      new ValuesSnippet(openApiObject.Paths.Select(pathItem =>
        new KeyValueSnippet(new KeySnippet(pathItem.Key), Controller.PathItemObjectHandler.Generate(pathItem.Key, pathItem.Value),
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