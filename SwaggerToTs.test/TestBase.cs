using SwaggerToTs.OpenApiDocument;

namespace SwaggerToTs.test;

public class TestBase
{

  private readonly string _root = Path.GetFullPath(Directory.GetCurrentDirectory() + "/../../../");

  private const string Input = "input.json";
  private const string Expect = "expect";
  private const string DebugPath = "dist";
  
  protected void InvokeThenAssert(string dir, string[]? inputArgs = null)
  {
   
      var subDir = Path.GetFullPath(_root + dir + "/");
      var swagger = Path.GetFullPath(subDir + Input);
      var dist = Path.GetFullPath(subDir + Expect);
      var args = new List<string>
      {
        "-s",
        swagger,
        "-d",
        dist
      };
      var debugArgs = args.GetRange(0, args.Count);
      debugArgs[3] = Path.GetFullPath(subDir + DebugPath);
      if (inputArgs != null)
      {
        args.AddRange(inputArgs);
        debugArgs.AddRange(inputArgs);
      }

      var writer = SwaggerToTs.Create(args.ToArray());
      var result = writer.Generate();
      var expect = new Dictionary<string, string>();
      foreach (string file in Directory.EnumerateFiles(dist, "*.ts", SearchOption.AllDirectories))
      {
        expect.Add(file, File.ReadAllText(file));
      }

      try
      {
        try
        {
          Assert.That(result.Count, Is.EqualTo(expect.Count));
        }
        catch (AssertionException)
        {
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
            message += $"miss: {string.Join(",", expects.Select(e => e.Replace(dist, "")))}";
          }
          if (results.Any())
          {
            message += $"should not create: {string.Join(",", results.Select(e => e.Replace(dist, "")))}";
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
            throw new AssertionException($"/{subDir.Split("/")[^2]}/{Expect}{key.Replace(dist, "")} :\n{e.Message}");
          }
        }
      }
      catch (AssertionException)
      {
        SwaggerToTs.Main(debugArgs.ToArray());
        throw;
      }
      

  }

  [OneTimeSetUp]
  public static void Main()
  {
    var cmd = System.Diagnostics.Process.Start("npm", " run format");
    cmd.WaitForExit();    
  }

}