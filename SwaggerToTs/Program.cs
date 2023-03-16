// See https://aka.ms/new-console-template for more information

using SwaggerToTs.OpenApiDocument;

namespace SwaggerToTs;

public static class SwaggerToTs
{
  public static void Main(string[] args)
  {

    var writer = Create(args);
    writer.Write(writer.Generate());

    Console.WriteLine($"ts file generate successfully to {writer.Options.Get<Dist>().Value}");
  }
  
  public static TsCodeWriter Create(string[] args)
  {
    var options = new Options(args);
    
    return TsCodeWriter.Create(options);
  }
}


