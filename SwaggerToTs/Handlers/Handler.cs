namespace SwaggerToTs.Handlers;

public class Handler
{
  protected Controller Controller;
  public Handler(Controller controller)
  {
    Controller = controller;
  }

  protected static string ToPascalCase(string name)
  {
    return char.ToUpperInvariant(name[0]) + name.Substring(1);
  }
 
}