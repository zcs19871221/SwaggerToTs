// See https://aka.ms/new-console-template for more information

using Newtonsoft.Json;
using SwaggerToTs.OpenAPIElements;

namespace SwaggerToTs;

public static class SwaggerToTs
{
  public static int Main(string[] args)
  {
    
    var options = new Options(args);
    if (!string.IsNullOrWhiteSpace(options.Get<Helper>().Value))
    {
      Console.WriteLine(options.Get<Helper>().Value);
      return 1;
    }

    var openApiObject = Create(options);
    var controller = new Controller(options, openApiObject.Components);

    controller.Write(controller.Generate(openApiObject));
    Console.WriteLine($"TypeScript files successfully generated to {options.Get<Dist>().Value} 👍");
    return 0;
  }
  
  public static OpenApiObject Create(Options options)
  {

    var swaggerJson = File.ReadAllText(options.Get<Swagger>().Value);

    var openApiObject = JsonConvert.DeserializeObject<OpenApiObject>(
                          swaggerJson,
                          new JsonSerializerSettings
                            { MetadataPropertyHandling = MetadataPropertyHandling.Ignore }) ??
                        throw new InvalidOperationException();

    return openApiObject;
  }
}


