using System.Text.RegularExpressions;
using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.Handlers;

public class PathItemObjectHandler:Handler
{
  
  private bool MatchTags(OperationObject operationObject, List<string> tags)
  {
    return tags.Any(tag =>
      operationObject.Tags.Exists(e => string.Equals(e, tag, StringComparison.CurrentCultureIgnoreCase)));
  }
  
  public bool Match(OperationObject operationObject)
  {
    var tagsToMatch = Controller.Options.Get<MatchTags>().Value;
    if (tagsToMatch.Count == 0) return true;
    return MatchTags(operationObject, tagsToMatch);
  }

  public bool Ignore(OperationObject operationObject)
  {
    var skipTags = Controller.Options.Get<IgnoreTags>().Value;

    if (skipTags.Count == 0) return false;
    return MatchTags(operationObject, skipTags);
  }

  
  public WrapperSnippet Generate(string url, PathItemObject pathItemObject)
  {
    var operations = new List<(string, OperationObject?)>
    {
      ("Get", pathItemObject.Get),
      ("Put", pathItemObject.Put),
      ("Post", pathItemObject.Post),
      ("Delete", pathItemObject.Delete),
      ("Options", pathItemObject.Options),
      ("Head", pathItemObject.Head),
      ("Patch", pathItemObject.Patch),
      ("Trace", pathItemObject.Trace)
    }.Where(e => e.Item2 != null && Match(e.Item2) && !Ignore(e.Item2)).Select(e =>
    {
      var operationObject = e.Item2;
      var method = e.Item1;
      if (operationObject == null)
      {
        throw new Exception("should not null");
      }

      var parameterKeys = operationObject.Parameters.Select(p =>
        _parameterObjectHandler.GetKey(p));
      operationObject.Parameters.AddRange(pathItemObject.Parameters.Where(p =>
        !parameterKeys.Contains(_parameterObjectHandler.GetKey(p))));
      var (exportName, fileLocate) = DecideOperationExtractInfo(url, method, operationObject);
      var operation =_operationObjectHandler.Generate(operationObject);
      var extracted = operation.RefactorAndSave(exportName, fileLocate, Controller);

      return WrapperSnippet.Create(
        new KeySnippet(method),
        extracted
      );

    }).ToList();

    var snippet = WrapperSnippet.Create(operations);
    
    snippet.AddComments(new List<(string, string?)>
    {
      (nameof(pathItemObject.Description), pathItemObject.Description),
      (nameof(pathItemObject.Summary), pathItemObject.Summary)
    });
    return snippet;
  }
  
  public (string,string) DecideOperationExtractInfo(string url, string method, OperationObject operationObject)
  {
    var fileName = operationObject.Tags.FirstOrDefault() ?? url.Split("/").ToList()
      .Find(e => !string.IsNullOrWhiteSpace(e) && !e.Contains("{") && !e.Contains("api"));
    if (string.IsNullOrWhiteSpace(fileName)) throw new Exception("operation FileName should not be empty");
    var exportName = !string.IsNullOrWhiteSpace(operationObject.OperationId)
      ? operationObject.OperationId
      : $"{fileName}{method}";
    exportName = ToPascalCase(exportName) + OperationEndsWith;
    while (!OperationExportNames.Add(exportName))
    {
      if (!Regex.IsMatch(exportName, "(\\d+)$"))
      {
        exportName += 1;
        continue;
      }

      exportName = Regex.Replace(exportName, "(\\d+)$", m => (int.Parse(m.Groups[1].ToString()) + 1).ToString());
    }

    return (exportName, fileName);
  }

  private OperationObjectHandler _operationObjectHandler;
  
  public string OperationEndsWith = "EP";

  private HashSet<string> OperationExportNames { get; set; } = new();

  private readonly ParameterObjectHandler _parameterObjectHandler;

  public PathItemObjectHandler(Controller controller) : base(controller)
  {
    _operationObjectHandler = new OperationObjectHandler(controller);
    _parameterObjectHandler = new ParameterObjectHandler(controller);
  }
}