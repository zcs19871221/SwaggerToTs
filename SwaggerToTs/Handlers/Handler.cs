using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.SchemaHandlers;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.Handlers;

public class Handler
{
  protected Controller Controller;
  public Handler(Controller controller)
  {
    Controller = controller;
  }
  
  public static string ToCamelCase(string name)
  {
    return char.ToLowerInvariant(name[0]) + name.Substring(1);
  }
  

  protected static string ToPascalCase(string name)
  {
    return char.ToUpperInvariant(name[0]) + name.Substring(1);
  }
 
}