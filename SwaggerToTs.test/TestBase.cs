using SwaggerToTs.OpenApiDocument;

namespace SwaggerToTs.test;

public class TestBase
{

  private readonly string _root = Path.GetFullPath(Directory.GetCurrentDirectory() + "/../../../");

  private const string Input = "input.json";
  private const string Expect = "expect";
  private const string DebugPath = "dist";
  
  protected void InvokeThenAssert(string dir, List<string>? tagsToIgnore = null, List<string>? tagsToMatch = null, bool? tryToGuessRequire = false, bool? nullValueIgnore = false, bool? schemaSaveToCommon = false)
  {
   
      var subDir = Path.GetFullPath(_root + dir + "/");
      var inputJson = Path.GetFullPath(subDir + Input);
      var outputDir = Path.GetFullPath(subDir + Expect);

      var text = File.ReadAllText(inputJson);
      var result = SwaggerToTs.CreateTsCode(text, outputDir, 80,tagsToIgnore, tagsToMatch, tryToGuessRequire, nullValueIgnore, schemaSaveToCommon).Generate();
      var expect = new Dictionary<string, string>();
      foreach (string file in Directory.EnumerateFiles(outputDir, "*.ts", SearchOption.AllDirectories))
      {
        expect.Add(file, File.ReadAllText(file));
      }

      try
      {
        Assert.That(result.Count, Is.EqualTo(expect.Count));
      }
      catch (AssertionException)
      {
        InvokeThenOutputForDebug(dir, tagsToIgnore, tagsToMatch, tryToGuessRequire, nullValueIgnore, schemaSaveToCommon);
        var expects = expect.Keys.ToList();
        var results = result.Keys.ToList();
        foreach (var expectKey in expect.Keys)
        {
          if (results.Contains(expectKey))
          {
            results.Remove(expectKey);
            expects.Remove(expectKey);
          }
        }
        
        var message = "";
        if (expects.Any())
        {
          message += $"miss: {string.Join(",", expects.Select(e => e.Replace(outputDir, "")))}";
        }
        if (results.Any())
        {
          message += $"should not create: {string.Join(",", results.Select(e => e.Replace(outputDir, "")))}";
        }
        throw new AssertionException("expect " + message);
      }
      foreach (var (key,value) in result)
      {
        try
        {
          Assert.That(value, Is.EqualTo(expect.GetValueOrDefault(key)));
        }
        catch (AssertionException e)
        {
          InvokeThenOutputForDebug(dir, tagsToIgnore, tagsToMatch, tryToGuessRequire, nullValueIgnore, schemaSaveToCommon);
          throw new AssertionException($"/{subDir.Split("/")[^2]}/{Expect}{key.Replace(outputDir, "")} :\n{e.Message}");
        }
      }

  }

  [OneTimeSetUp]
  public static void Main()
  {
    var cmd = System.Diagnostics.Process.Start("npm", " run format");
    cmd.WaitForExit();    
  }

  private void InvokeThenOutputForDebug(string dir, List<string>? tagsToIgnore = null, List<string>? tagsToMatch = null, bool? tryToGuessRequire = false, bool? nullValueIgnore = false, bool? schemaSaveToCommon = false)
  {
    var subDir = Path.GetFullPath(_root + dir + "/");
    var inputJson = Path.GetFullPath(subDir + Input);
    var outputDir = Path.GetFullPath(subDir + DebugPath);

    var text = File.ReadAllText(inputJson);
    var result = SwaggerToTs.CreateTsCode(text, outputDir, 80,tagsToIgnore, tagsToMatch, tryToGuessRequire, nullValueIgnore, schemaSaveToCommon);
    TsCodeWriter.Get().Write(result.Generate());
  }

}