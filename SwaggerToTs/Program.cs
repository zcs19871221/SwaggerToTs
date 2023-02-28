// See https://aka.ms/new-console-template for more information

using Newtonsoft.Json;
using SwaggerToTs.OpenApiDocument;

namespace SwaggerToTs;

public static class SwaggerToTs
{
  public static void Main(string[] args)
  {
    var swaggerLocation = Path.Combine(Directory.GetCurrentDirectory(), "swagger.json");
    var targetDirectory = Path.Combine(Directory.GetCurrentDirectory(), "apis");
    List<string>? tagsToIgnore = null;
    List<string>? tagsToMatch = null;
    var printWidth = 80;
    for (var i = 0; i < args.Length; i++)
    {
      string GetOptValue()
      {
        return args[++i].Trim();
      }

      var opt = args[i].Trim().Replace("-", "").Trim();
      switch (opt)
      {
        case "i":
        case "input":
          swaggerLocation = Path.GetFullPath(GetOptValue());
          break;
        case "o":
        case "output":
          targetDirectory = Path.GetFullPath(GetOptValue());
          break;
        case "s":
        case "skip":
          tagsToIgnore = GetOptValue().Split(",").ToList();
          break;
        case "p":
        case "printWidth":
          printWidth = int.Parse(GetOptValue());
          break;
        case "m":
        case "match":
          tagsToMatch = GetOptValue().Split(",").ToList();
          break;
      }
    }

    var text = File.ReadAllText(swaggerLocation);

    var writer = CreateTsCode(text, targetDirectory, printWidth, tagsToIgnore, tagsToMatch);
    writer.Write(writer.Generate());

    Console.WriteLine($"ts file generate successfully to {targetDirectory}");
  }

  public static TsCodeWriter CreateTsCode(string swaggerJson, string targetDirectory, int printWidth = 80,
    List<string>? tagsToIgnore = null, List<string>? tagsToMatch = null)
  {
    var openApiDocument = JsonConvert.DeserializeObject<OpenApiObject>(
                            swaggerJson,
                            new JsonSerializerSettings
                              { MetadataPropertyHandling = MetadataPropertyHandling.Ignore }) ??
                          throw new InvalidOperationException();
    return TsCodeWriter.Create(targetDirectory, printWidth, tagsToIgnore, tagsToMatch,
      openApiDocument);
  }
}