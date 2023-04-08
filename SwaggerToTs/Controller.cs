using System.Text;
using System.Text.RegularExpressions;
using SwaggerToTs.Handlers;
using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.SchemaHandlers;
using SwaggerToTs.Snippets;

namespace SwaggerToTs;
public class Controller
{
  public List<ValueSnippet> IsolateSnippets = new List<ValueSnippet>();

  public Dictionary<string, ValueSnippet> RefMappingIsolate = new();

  public Options Options;

  public ComponentsObject? Components;
  public OpenApiObjectHandler OpenApiObjectHandler { get; }
  public ComponentsObjectHandler ComponentsObjectHandler { get; }
  public PathItemObjectHandler PathItemObjectHandler { get; set; }
  public OperationObjectHandler OperationObjectHandler { get; set; }
  public ParameterObjectHandler ParameterObjectHandler { get; set; }
  public RequestBodyObjectHandler RequestBodyObjectHandler { get; set; }
  public ResponseObjectHandler ResponseObjectHandler { get; set; }
  
  public HeaderObjectHandler HeaderObjectHandler { get; set; }
  public AllOfHandler AllOfHandler { get; set; }
  public AnyHandler AnyHandler { get; set; }
  public AnyOfHandler AnyOfHandler { get; set; }
  public ArrayHandler ArrayHandler { get; set; }
  public BoolHandler BoolHandler { get; set; }
  public EnumHandler EnumHandler { get; set; }
  
  public NumberHandler NumberHandler { get; set; }
  public ObjectHandler ObjectHandler { get; set; }
  public OneOfHandler OneOfHandler { get; set; }
  
  public StringHandler StringHandler { get; set; }
  public UnknownHandler UnknownHandler { get; set; }
  public Dictionary<string, string> ReferenceMappingShortName { get; set; }

  public List<SchemaObjectHandler> SchemaHandlers;
  
  public ValueSnippet SelectThenConstruct(SchemaObject schema)
  {
    var handler = SchemaHandlers.Find(h => h.IsMatch(schema));
    return (handler ?? throw new Exception("cant find handler for schema")).Construct(schema);
  }
  
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
    AllOfHandler = new AllOfHandler(this);
    AnyHandler = new AnyHandler(this);
    AnyOfHandler = new AnyOfHandler(this);
    ArrayHandler = new ArrayHandler(this);
    BoolHandler = new BoolHandler(this);
    EnumHandler = new EnumHandler(this);
    NumberHandler = new NumberHandler(this);
    ObjectHandler = new ObjectHandler(this);
    OneOfHandler = new OneOfHandler(this);
    UnknownHandler = new UnknownHandler(this);
    StringHandler = new StringHandler(this);
    SchemaHandlers = new List<SchemaObjectHandler>()
    {
      AllOfHandler,
      AnyHandler,
      AnyOfHandler,
      ArrayHandler,
      BoolHandler,
      EnumHandler,
      NumberHandler,
      ObjectHandler,
      OneOfHandler,
      UnknownHandler,
      StringHandler
    };
  }

  public static string OneOfName = "OneOf";
  public static string AnyOfName = "AnyOf";
  public static string NonNullAsRequired = "NonNullAsRequired";
  private static readonly string HelperContent = $@"/* eslint-disable @typescript-eslint/no-explicit-any */
type IntersectionTuple<S, T extends any[]> = T extends [infer F, ...infer R]
  ? [S & F, ...IntersectionTuple<S, R>]
  : T;

type Permutations<T extends readonly unknown[]> = T['length'] extends 0 | 1
  ? T
  : T extends [infer F, ...infer R]
  ? [F, ...IntersectionTuple<F, Permutations<R>>, ...Permutations<R>]
  : T;

type AllKeysOf<T> = T extends any ? keyof T : never;

type ProhibitKeys<K extends keyof any> = {{ [P in K]?: never }};

export type {OneOfName}<T extends any[]> = {{
  [K in keyof T]: T[K] &
    ProhibitKeys<Exclude<AllKeysOf<T[number]>, keyof T[K]>>;
}}[number];

export type {AnyOfName}<T extends any[]> = OneOf<Permutations<T>>;

type NullKeys<T> = {{
    [k in keyof T]: null extends T[k] ? k : never;
}}[keyof T];

export type {NonNullAsRequired}<T> = T extends (infer U)[]
  ? NonNullAsRequired<U>[]
  : T extends object
  ? Pick<T, NullKeys<T>> & {{
  [K in Exclude<keyof T, NullKeys<T>>]-?: NonNullAsRequired<T[K]>;
    }}
  : T;
";

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