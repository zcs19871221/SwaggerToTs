using System.Text;
using System.Text.RegularExpressions;
using SwaggerToTs.Handlers;
using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs;
public class Controller
{
  public List<ValueSnippet> IsolateSnippets = new List<ValueSnippet>();

  public Dictionary<string, ValueSnippet> RefMappingIsolate = new();

  public Options Options;

  public ComponentsObject? Components;

  public HashSet<string> OperationIds { get; set; } = new();
  
  public OpenApiObjectHandler OpenApiObjectHandler { get; }
  public PathItemObjectHandler PathItemObjectHandler { get; set; }
  public OperationObjectHandler OperationObjectHandler { get; set; }
  public ParameterObjectHandler ParameterObjectHandler { get; set; }
  public RequestBodyObjectHandler RequestBodyObjectHandler { get; set; }
  public ResponseObjectHandler ResponseObjectHandler { get; set; }
  
  public HeaderObjectHandler HeaderObjectHandler { get; set; }
  public SchemaObjectHandler SchemaObjectHandler { get; set; }
  public  ObjectHandler ObjectHandler { get; set; }

  
  
  public static string ToCamelCase(string name)
  {
    return char.ToLowerInvariant(name[0]) + name.Substring(1);
  }

  protected static string ToPascalCase(string name)
  {
    return char.ToUpperInvariant(name[0]) + name.Substring(1);
  }

  public Controller(Options options, ComponentsObject? components)
  {
    Options = options;
    Components = components;
    OpenApiObjectHandler = new OpenApiObjectHandler(this);
    PathItemObjectHandler = new PathItemObjectHandler(this);
    OperationObjectHandler = new OperationObjectHandler(this);
    ParameterObjectHandler = new ParameterObjectHandler(this);
    RequestBodyObjectHandler = new RequestBodyObjectHandler(this);
    ResponseObjectHandler = new ResponseObjectHandler(this);
    SchemaObjectHandler = new SchemaObjectHandler(this);
    HeaderObjectHandler = new HeaderObjectHandler(this);

  }
  

  private string CreateImport(List<string> imports, string fileToImport,
    bool exclusiveRow = false)
  {
    var separator = exclusiveRow ? NewLine + "  " : " ";
    var content =
      $@"import {{{separator}{string.Join("," + separator, imports)}{(exclusiveRow ? NewLine : " ")}}} from './{fileToImport}';";
    if (!exclusiveRow && content.Length > Options.Get<PrintWidth>().Value) return CreateImport(imports, fileToImport, true);

    return content;
  }

  public void Write(Dictionary<string, string> contents)
  {
    var dir = new DirectoryInfo(Options.Get<Dist>().Value);
    if (dir.Exists) dir.Delete(true);

    foreach (var (fileLocate, content) in contents)
    {
      Directory.CreateDirectory(Path.GetDirectoryName(fileLocate) ?? throw new InvalidOperationException());
      File.WriteAllText(fileLocate, content);
    }
  }

  public Dictionary<string, string> Generate(OpenApiObject openApiObject)
  {
    OpenApiObjectHandler.Generate(openApiObject);
    Dictionary<string, string> fileMappingText = new();
    foreach (var (fileLocate, isolateSnippets) in IsolateSnippets.GroupBy(e => e.FileLocate, (s, snippets) => (s, snippets.ToList())))
    {
      
      isolateSnippets.Sort((x, y) =>
      {
        if (x.Priority != y.Priority) return x.Priority.CompareTo(y.Priority);

        return string.Compare(x.ExportName, y.ExportName, StringComparison.Ordinal);
      });
      SortedDictionary<string, HashSet<string>> imports = new();
      StringBuilder contents = new();
      StringBuilder importBlock = new();
      HashSet<string> dup = new();

      foreach (var isolateSnippet in isolateSnippets)
      {
        if (string.IsNullOrEmpty(isolateSnippet.ExportName) || string.IsNullOrEmpty(isolateSnippet.FileLocate)) throw new Exception("empty Export name or Locate");

        if (dup.Contains(isolateSnippet.ExportName))
          throw new Exception($"dup export name {isolateSnippet.ExportName} in {fileLocate}");

        dup.Add(isolateSnippet.ExportName);
        var dependencies = new List<ValueSnippet>();
        if (Options.Get<NonNullAsRequired>().Value)
        {
          var responses = isolateSnippet.UsedBy.Where(e => e.CodeLocate == CodeLocate.Response).ToList();
          var requests = isolateSnippet.UsedBy.Where(e => e.CodeLocate == CodeLocate.Response);
          ;
          if (responses.Any() && requests.Any())
          {
            imports.GetOrCreate("Helper").Add("NonNullAsRequired");
            fileMappingText.TryAdd(GetFullPath("Helper"), HelperContent);            
            foreach (var response in responses)
            {
              response.Generic = "NonNullAsRequired";
            }
          }
        }
        var content = isolateSnippet.Generate(Options, dependencies);
        foreach (var codeDependency in dependencies)
        {
          if (codeDependency.ExportName == null || string.IsNullOrEmpty(codeDependency.FileLocate)) throw new Exception("exportName or fileLocate should not have empty valueSnippet");

          if (codeDependency.FileLocate != fileLocate) imports.GetOrCreate(codeDependency.FileLocate).Add(codeDependency.ExportName);
        }

        if (contents.Length > 0) contents.AppendLine();

        contents.AppendLine(content);
      }

      foreach (var (fileToImport, importName) in imports)
      {
        var x = importName.ToList();
        x.Sort((s, s1) => string.Compare(s, s1, StringComparison.Ordinal));
        importBlock.AppendLine(CreateImport(x, fileToImport));
      }

      if (importBlock.Length > 0) importBlock.AppendLine();

      var target = GetFullPath(fileLocate);
      fileMappingText.Add(target, Warning + NewLine + NewLine + importBlock.ToString() + contents.ToString());
    }


    return fileMappingText;
  }

  private string GetFullPath(string fileLocate )
  {
    return  Path.Combine(Options.Get<Dist>().Value, fileLocate + ".ts");
  }
  
  public static readonly string NewLine = "\n";

  
  private const string Warning = @"/**
 * This file was auto-generated by the program based on the back-end data structrue.
 * Do not make direct changes to the file.
 */";
  
}