using System.Text;
using SwaggerToTs.Handlers;
using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.SchemaHandlers;
using SwaggerToTs.Snippets;

namespace SwaggerToTs;
public class Controller
{
  public readonly HashSet<ValueSnippet> IsolateSnippets = new ();

  public readonly Dictionary<string, ValueSnippet> RefMappingIsolate  = new();

  public readonly Options Options;

  public ComponentsObject? Components;
  private OpenApiObjectHandler OpenApiObjectHandler { get; }
  private ComponentsObjectHandler ComponentsObjectHandler { get; }
  public PathItemObjectHandler PathItemObjectHandler { get; }
  public OperationObjectHandler OperationObjectHandler { get; }
  public ParameterObjectHandler ParameterObjectHandler { get;  }
  public RequestBodyObjectHandler RequestBodyObjectHandler { get;  }
  public ResponseObjectHandler ResponseObjectHandler { get;  }
  public HeaderObjectHandler HeaderObjectHandler { get;  }
  public Dictionary<string, string> ReferenceMappingShortName { get; private set; } = null!;

  public SchemaObjectHandlerWrapper SchemaObjectHandlerWrapper { get;  }

  public CodeLocation CurrentLocation { get; set; }
  public Controller(Options options)
  {
    Options = options;
    OpenApiObjectHandler = new OpenApiObjectHandler(this);
    ComponentsObjectHandler = new ComponentsObjectHandler(this);
    PathItemObjectHandler = new PathItemObjectHandler(this);
    OperationObjectHandler = new OperationObjectHandler(this);
    ParameterObjectHandler = new ParameterObjectHandler(this);
    RequestBodyObjectHandler = new RequestBodyObjectHandler(this);
    ResponseObjectHandler = new ResponseObjectHandler(this);
    HeaderObjectHandler = new HeaderObjectHandler(this);
    SchemaObjectHandlerWrapper = new SchemaObjectHandlerWrapper(this);
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

  public Dictionary<string, string> Construct(OpenApiObject openApiObject)
  {
    Components = openApiObject.Components;
    ReferenceMappingShortName = ComponentsObjectHandler.Construct(openApiObject.Components);
    OpenApiObjectHandler.Construct(openApiObject);
    Dictionary<string, string> fileMappingText = new();
    foreach (var (fileLocate, isolateSnippets) in IsolateSnippets.GroupBy(e => e.FileLocate, (s, snippets) => (s, snippets.ToList())))
    {
      if (fileLocate == null)
      {
        throw new Exception("fileLocate should not null");
      }
      isolateSnippets.Sort((x, y) => x.Priority != y.Priority ? x.Priority.CompareTo(y.Priority) : string.Compare(x.ExportName, y.ExportName, StringComparison.Ordinal));
      SortedDictionary<string, HashSet<string>> fileMappingExportNames = new();
      StringBuilder contents = new();
      StringBuilder importBlock = new();
      var generatingInfo = new GeneratingInfo(this);
      foreach (var isolateSnippet in isolateSnippets)
      {
        if (string.IsNullOrEmpty(isolateSnippet.ExportName) || string.IsNullOrEmpty(isolateSnippet.FileLocate)) throw new Exception("empty Export name or Locate");
        var content = isolateSnippet.Generate(generatingInfo);
        if (contents.Length > 0) contents.AppendLine();
        contents.AppendLine(content);
      }
      generatingInfo.AggregateImports(fileMappingExportNames, fileLocate);
      foreach (var (fileToImport, importName) in fileMappingExportNames)
      {
        var x = importName.ToList();
        x.Sort((s, s1) => string.Compare(s, s1, StringComparison.Ordinal));
        importBlock.AppendLine(CreateImport(x, fileToImport));
      }

      if (importBlock.Length > 0) importBlock.AppendLine();

      var target = GetFullPath(fileLocate);
      fileMappingText.Add(target, Warning + NewLine + NewLine + importBlock + contents);
      if (generatingInfo.HelperNames.Count > 0)
      {
        fileMappingText.Add(GetFullPath(Helpers.FileName), Helpers.HelperContent);
      }
    }


    return fileMappingText;
  }

  private string GetFullPath(string fileLocate )
  {
    return Path.Combine(Options.Get<Dist>().Value, fileLocate + ".ts");
  }

  private const string NewLine = "\n";


  private const string Warning = @"/**
 * This file was auto-generated by the program based on the back-end data structure.
 * Do not make direct changes to the file.
 */";
  
}
