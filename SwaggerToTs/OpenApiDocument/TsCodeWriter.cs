using System.Text;
using Newtonsoft.Json;

namespace SwaggerToTs.OpenApiDocument;

public class TsCodeWriter
{
  private static TsCodeWriter? _writer;
  private readonly List<TsCodeElement> _codes = new();
  private readonly OpenApiObject _openApiObject;

  public readonly HashSet<string> OperationExportNames = new();
  public readonly Dictionary<string, TsCodeElement> RefMappingCode = new();
  public Options Options { get; set; }

  private const string Warning = @"/**
 * This file was auto-generated by the program based on the back-end data structrue.
 * Do not make direct changes to the file.
 */";

  private TsCodeWriter(
    OpenApiObject openApiObject,
    Options options
    )
  {
    Options = options;
    _openApiObject = openApiObject;
    ComponentsObject = openApiObject.Components;
  }

  public static string OneOfName = "OneOf";
  public static string AnyOfName = "AnyOf";

  public  const string SchemaFile = "data-schema";
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

";

  private const string HelperLocate = "helper";
  
  public ComponentsObject? ComponentsObject { get; set; }
  public HashSet<string> OperationIds { get; set; } = new();

  public static TsCodeWriter Get()
  {
    if (_writer == null) throw new Exception("writer is null");

    return _writer;
  }

  public static TsCodeWriter Create(Options options)
  {
    var swaggerJson = File.ReadAllText(options.Get<Swagger>().Value);

    var openApiObject = JsonConvert.DeserializeObject<OpenApiObject>(
                          swaggerJson,
                          new JsonSerializerSettings
                            { MetadataPropertyHandling = MetadataPropertyHandling.Ignore }) ??
                        throw new InvalidOperationException();
    if (_writer == null || _writer._openApiObject != openApiObject)
      _writer = new TsCodeWriter(openApiObject, options);
    return _writer;
  }


  public bool Match(OperationObject operationObject)
  {
    var tagsToMatch = Options.Get<MatchTags>().Value;
    if (tagsToMatch.Count == 0) return true;
    return MatchTags(operationObject, tagsToMatch);
  }

  public bool Ignore(OperationObject operationObject)
  {
    var skipTags = Options.Get<IgnoreTags>().Value;

    if (skipTags.Count == 0) return false;
    return MatchTags(operationObject, skipTags);
  }

  private bool MatchTags(OperationObject operationObject, List<string> tags)
  {
    return tags.Any(tag =>
      operationObject.Tags.Exists(e => string.Equals(e, tag, StringComparison.CurrentCultureIgnoreCase)));
  }

  public void Add(TsCodeElement element)
  {
    _codes.Add(element);
  }
  private string CreateImport(List<string> imports, string fileToImport,
    bool exclusiveRow = false)
  {
    var separator = exclusiveRow ? TsCodeElement.NewLine + "  " : " ";
    var content =
      $@"import {{{separator}{string.Join("," + separator, imports)}{(exclusiveRow ? TsCodeElement.NewLine : " ")}}} from './{fileToImport}';";
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

  public Dictionary<string, string> Generate()
  {
    _openApiObject.GenerateTsCode();
    Dictionary<string, List<TsCodeElement>> fileMappingOutput = new();
    Dictionary<string, string> fileMappingText = new();
    foreach (var code in _codes)
    {
      fileMappingOutput.GetOrCreate(code.FileLocate ?? throw new Exception("should have fileLocate")).Add(code);
    }

    foreach (var (fileLocate, outputs) in fileMappingOutput)
    {
      SortedDictionary<string, HashSet<string>> imports = new();
      StringBuilder contents = new();
      StringBuilder importBlock = new();
      HashSet<string> dup = new();
      outputs.Sort((x, y) =>
      {
        if (x.Priority != y.Priority) return x.Priority.CompareTo(y.Priority);

        return string.Compare(x.ExportName, y.ExportName, StringComparison.Ordinal);
      });
      foreach (var output in outputs)
      {
        if (string.IsNullOrEmpty(output.ExportName)) throw new Exception("empty Export name");

        if (dup.Contains(output.ExportName))
          throw new Exception($"dup export name {output.ExportName} in {fileLocate}");

        dup.Add(output.ExportName);
        foreach (var codeDependency in output.ExtractedCodeImports)
        {
          if (codeDependency.ExportName == null || string.IsNullOrEmpty(codeDependency.FileLocate)) throw new Exception("exportName or fileLocate should not have empty value");

          if (codeDependency.FileLocate != fileLocate) imports.GetOrCreate(codeDependency.FileLocate).Add(codeDependency.ExportName);
        }

        foreach (var helperName in output.ExtractedCodeImportedHelpers)
        {
          imports.GetOrCreate(HelperLocate).Add(helperName);
          fileMappingText.TryAdd(GetFullPath(HelperLocate), HelperContent);
        }

        if (contents.Length > 0) contents.AppendLine();

        contents.AppendLine(output.ExportContent);
      }

      foreach (var (fileToImport, importName) in imports)
      {
        var x = importName.ToList();
        x.Sort((s, s1) => string.Compare(s, s1, StringComparison.Ordinal));
        importBlock.AppendLine(CreateImport(x, fileToImport));
      }

      if (importBlock.Length > 0) importBlock.AppendLine();

      var target = GetFullPath(fileLocate);
      fileMappingText.Add(target, Warning + TsCodeElement.NewLine + TsCodeElement.NewLine + importBlock + contents);
    }


    return fileMappingText;
  }

  private string GetFullPath(string fileLocate )
  {
    return  Path.Combine(Options.Get<Dist>().Value, fileLocate + ".ts");
  }
}