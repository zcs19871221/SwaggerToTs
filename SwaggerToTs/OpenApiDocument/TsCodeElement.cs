using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace SwaggerToTs.OpenApiDocument;

public abstract class TsCodeElement
{
  private static readonly List<string> ReservedKeyWords = new()
  {
    "Record"
  };

  private readonly List<(string, string)> _comments = new();
  private HashSet<TsCodeElement> _imports = new();
  public  List<string> HelpersToImport = new();
  public  List<string> HelpersForWrite = new();

  private string? _exportName;
  private bool _isContentArray;

  

  private bool _isExtracted;

  private bool _isGenerated;
  private string? _name;
  public HashSet<TsCodeElement> Imports { get; private set; } = new();
  public string? ExportContent { get; private set; }

  public static string NewLine = "\n";
  public List<string> Extends { get; } = new();

  public HashSet<TsCodeElement> References { get; } = new();

  public string? ExportName
  {
    get => _exportName;
    set => _exportName = FormatExportName(value);
  }


  public string? Name
  {
    get => _name;
    set => _name = FormatName(value);
  }

  public string? FileLocate { get; set; }

  public int Priority { get; set; }

  public string? DefaultFileLocate { get; set; }

  public ExportType ExportTypeValue { get; set; } = ExportType.Interface;


  public bool ReadOnly { get; set; }

  public string Contents { get; set; } = "";

  public bool? Optional { get; set; }

  public bool? OverrideHeadOptional { get; set; } = null;

  public string? Summary { get; set; }
  public string? Description { get; set; }

  [JsonProperty(PropertyName = "$ref")] public string? Reference { get; set; }

  private CodeType _codeType => Name == null ? CodeType.Body : CodeType.Head;

  private string FormatExportName(string? value)
  {
    if (string.IsNullOrWhiteSpace(value)) throw new Exception("export Name should not be null");
    var exportName = char.ToUpperInvariant(value[0]) + value.Substring(1);
    if (ReservedKeyWords.Contains(exportName)) exportName += "Type";
    return exportName;
  }


  private static string WriteBrackets(string content)
  {
    if (!string.IsNullOrWhiteSpace(content) && !content.StartsWith("{"))
    {
      var contents = content.Split(NewLine).ToList();
      for (var i = 0; i < contents.Count; i++) contents[i] = $"  {contents[i]}";

      contents.Insert(0, "{");
      contents.Add("}");
      return string.Join(NewLine, contents);
    }

    return content;
  }

  private bool IsEmpty()
  {
    if (string.IsNullOrWhiteSpace(Contents) && string.IsNullOrWhiteSpace(ExportContent)) return true;

    return false;
  }

  private void DefaultContentMerger(TsCodeElement code)
  {
    Contents += code.ToString();
  }

  public TsCodeElement Merge(TsCodeElement code, Action<TsCodeElement>? mergeHandler = null)
  {
    if (code.IsEmpty()) return this;

    var mergeType = _codeType + "-" + code._codeType;
    switch (mergeType)
    {
      case "Head-Head":
        throw new Exception("not correct merge");
      case "Head-Body":
        if (code is SchemaObject { SchemaType: SchemaTypeEnums.Enum, ExportName: null } && TsCodeWriter.Get().Options.Get<EnumUseEnum>().Value)
        {
          code.ExtractTo(Name, TsCodeWriter.SchemaFile);
        }
        AppendImports(code);
        AppendComments(code);
        // if (code.Optional !=null && Optional == null)
        if (code.OverrideHeadOptional !=null)
        {
          Optional = code.OverrideHeadOptional;
        }
        if (code is SchemaObject { SchemaType: SchemaTypeEnums.Array, ExportName: null })
        {
          _isContentArray = true;
        }
        break;
      case "Body-Body":
        AppendImports(code);
        AppendComments(code);
        ExportTypeValue = code.ExportTypeValue;
        break;
      case "Body-Head":
        AppendImports(code);
        break;
      default:
        throw new Exception($"can' t handle mergeType {mergeType}");
    }

    mergeHandler ??= DefaultContentMerger;

    mergeHandler(code);

    return this;
  }

  private string? FormatName(string? name)
  {
    if (name == null) return null;

    if (this is not TsCodeFragment) name = ToCamelCase(name);
    if (Regex.IsMatch(name, @"^[a-zA-Z_\d$.]+$")) return name;

    return $"'{name}'";
  }
  
  private StringBuilder WriteComments()
  {
    StringBuilder sb = new();
    foreach (var comment in _comments)
    {
      var commentBody = comment.Item2.Replace("*/", "*\\/");
      var lines = Regex.Split(commentBody, "\n|\r\n");
      sb.AppendLine($"{(sb.Length == 0 ? "/**" : " *")}");
      sb.AppendLine($" * @{comment.Item1} {lines.First()}");
      for (var i = 1; i < lines.Length; i++) sb.AppendLine($" * {lines[i]}");
    }

    if (sb.Length > 0) sb.Append(" */");

    return sb;
  }

  private string BodyToString(string name, string connector, string content)
  {
    return $"{name}{connector}{content}";
  }

  public string GenerateCodeBody()
  {
    var name = "";
    var connector = "";
    var content = Contents;
    if (_codeType == CodeType.Head)
    {
      name = $"{(ReadOnly ? "readonly " : "")}{Name}{(Optional ?? true ? "?" : "")}";
      connector = ": ";
      content += ";";
      if (_isContentArray) content = $"readonly {content}";
    }

    return BodyToString(name, connector, content);
  }

  public override string ToString()
  {
    var body = GenerateCodeBody();
    return ToString(body);
  }

  private string ExportToString()
  {
    string name;
    string connector = " ";
    var content = GenerateCodeBody();
    var exportType = ExportTypeValue;
    switch (exportType)
    {
      case ExportType.Interface:
        var exportName = ExportName;
        if (Extends.Count > 0)
        {
          Extends.Sort((a, b) => string.Compare(a, b, StringComparison.Ordinal));
          exportName += $" extends {string.Join(", ", Extends)}";
        }

        name = $"export interface {exportName}";
        content = WriteBrackets(content);
        connector = " ";
        break;
      case ExportType.Type:
        name = $"export type {ExportName}";
        connector = " = ";
        if (content.Contains("|") && $"{name}{connector}{content};".Length > TsCodeWriter.Get().Options.Get<PrintWidth>().Value)
        {
          var separator = NewLine + "  | ";
          connector = " =";
          content = separator + string.Join(separator, content.Split("|").Select(e => e.Trim()));
        }

        content += ";";
        break;
      case ExportType.Enum:
        name = $"export enum {ExportName}";
        break;

      default:
        throw new Exception("To be exported code doesn't  have export type");
    }

    return ToString(BodyToString(name, connector, content));
  }

  private string ToString(string codeBody)
  {
    if (codeBody.Length == 0) return "";


    var comments = WriteComments();
    if (comments.Length > 0) return comments + NewLine + codeBody;

    return codeBody;
  }

  private void AppendImports(TsCodeElement t)
  {
    foreach (var tDependency in t._imports)
    {
      _imports.Add(tDependency);
    }

    foreach (var helpers in t.HelpersToImport)
    {
      HelpersToImport.Add(helpers);
    }
  }

  private void AppendComments(TsCodeElement code)
  {
    foreach (var (key, comment) in code._comments) AddComment(key, comment);
    code._comments.Clear();
  }


  public TsCodeElement AddComment(string name, string? value)
  {
    if (!string.IsNullOrEmpty(value)) _comments.Add((name, value));

    return this;
  }

  public TsCodeElement ExtractTo(string? exportName = null, string? fileLocate = null)
  {
    if (IsEmpty() || _isExtracted) return this;


    if (exportName != null) ExportName = exportName;

    if (fileLocate != null) FileLocate = fileLocate;

    if (string.IsNullOrWhiteSpace(ExportName))
    {
      throw new Exception("export Name should not be empty");
    }

    foreach (var dep in _imports) dep.References.Add(this);

    Imports = _imports;

    ExportContent = ExportToString();
    Contents = ExportName;
    Name = null;
    _comments.Clear();
    _imports = new HashSet<TsCodeElement> { this };
    HelpersForWrite = HelpersToImport;
    HelpersToImport = new();
    TsCodeWriter.Get().Add(this);
    _isExtracted = true;
    return this;
  }

  protected abstract void ValidateOpenApiDocument();

  protected abstract TsCodeElement CreateTsCode();

  private TsCodeElement DoCreateTsCode()
  {
    ValidateOpenApiDocument();
    AddComment(nameof(Summary), Summary).AddComment(nameof(Description), Description);
    return CreateTsCode();
  }

  public TsCodeElement GetRefOrSelfIfNotExists()
  {
    return TsCodeWriter.Get().ComponentsObject?.GetRefMaybe(Reference) ?? this;
  }

  public TsCodeElement GenerateTsCode()
  {
    if (_isGenerated) return this;

    TsCodeElement target;
    if (Reference != null && TsCodeWriter.Get().RefMappingCode.TryGetValue(Reference, out var exist)) return exist;

    if (Reference != null)
    {
      var element = TsCodeWriter.Get().ComponentsObject?.GetRef(Reference);
 
      if (element == null) throw new Exception("ref element should not be null");
      target = element.DoCreateTsCode().ExtractTo();
      TsCodeWriter.Get().RefMappingCode.Add(Reference, target);
      element._isGenerated = true;
    }
    else
    {
      target = DoCreateTsCode();
      _isGenerated = true;
    }


    return target;
  }

  public static TsCodeElement CreateFragment<T>(IDictionary<string, T> dict, bool isReadonly = false,
    Action<string, TsCodeElement, TsCodeElement>? onItemGenerated = null) where T : TsCodeElement
  {
    var sorted = dict.OrderBy(e => e.Key);
    List<TsCodeElement> tsCodes = new();
    foreach (var (key, c) in sorted)
    {
      var code = c.GenerateTsCode();
      var t = new TsCodeFragment { Name = key, ReadOnly = isReadonly, Optional = false};
      if (onItemGenerated != null) onItemGenerated(key, code, t);
      t.Merge(code);
      tsCodes.Add(t);
    }

    var result = CreateFragment(tsCodes);
    result.Contents = WriteBrackets(result.Contents);
    return result;
  }

  public static string ToCamelCase(string name)
  {
    return char.ToLowerInvariant(name[0]) + name.Substring(1);
  }

  protected static string ToPascalCase(string name)
  {
    return char.ToUpperInvariant(name[0]) + name.Substring(1);
  }

  public static TsCodeElement CreateFragment(IEnumerable<TsCodeElement> codes,
    Action<TsCodeElement, TsCodeElement>? mergeHandler = null)
  {
    var fragment = new TsCodeFragment
    {
      ExportTypeValue = ExportType.Interface
    };
    mergeHandler ??= (item, _) =>
      fragment.Contents += (
        fragment.Contents.Length > 0
          ? NewLine
          : "") + item;
    foreach (var c in codes)
    {
      var code = c.GenerateTsCode();
      fragment.Merge(code, item => { mergeHandler(item, fragment); });
    }

    return fragment;
  }

  private enum CodeType
  {
    Head,
    Body
  }
}

public enum ExportType
{
  Interface,
  Type,
  Enum,
}