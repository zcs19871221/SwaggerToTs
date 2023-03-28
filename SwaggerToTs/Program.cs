// See https://aka.ms/new-console-template for more information

using SwaggerToTs.TypeScriptGenerator;

namespace SwaggerToTs;

public static class SwaggerToTs
{
  public static int Main(string[] args)
  {

    var writer = Create(args);
    if (!string.IsNullOrWhiteSpace(writer.Options.Get<Helper>().Value))
    {
      Console.WriteLine(writer.Options.Get<Helper>().Value);
      return 1;
    }
    writer.Write(writer.Generate());
    Console.WriteLine($"TypeScript files successfully generated to {writer.Options.Get<Dist>().Value} 👍");
    return 0;
  }
  
  public static TsCodeWriter Create(string[] args)
  {
    var options = new Options(args);
    
    return TsCodeWriter.Create(options);
  }
}


