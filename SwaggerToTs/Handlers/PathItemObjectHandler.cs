using System.Text.RegularExpressions;
using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.Handlers;

public class PathItemObjectHandler : Handler
{
  private static bool MatchTags(OperationObject operationObject, IEnumerable<string> tags)
  {
    return tags.Any(tag =>
      operationObject.Tags.Exists(e => string.Equals(e, tag, StringComparison.CurrentCultureIgnoreCase)));
  }

  private bool Match(OperationObject operationObject)
  {
    var tagsToMatch = Controller.Options.Get<MatchTags>().Value;
    return tagsToMatch.Count == 0 || MatchTags(operationObject, tagsToMatch);
  }

  private bool Ignore(OperationObject operationObject)
  {
    var skipTags = Controller.Options.Get<IgnoreTags>().Value;

    return skipTags.Count != 0 && MatchTags(operationObject, skipTags);
  }


  public ValueSnippet Construct(string url, PathItemObject pathItemObject)
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
      if (operationObject == null) throw new Exception("should not null");

      var parameterObjectHandler = Controller.ParameterObjectHandler;
      var parameterKeys = operationObject.Parameters.Select(p =>
        parameterObjectHandler.GetKey(p));
      operationObject.Parameters.AddRange(pathItemObject.Parameters.Where(p =>
        !parameterKeys.Contains(parameterObjectHandler.GetKey(p))));
      
      var (exportName, fileLocate) = DecideOperationExtractInfo(url, method, operationObject);
      var operation = Controller.OperationObjectHandler.Construct(operationObject,
        Regex.Replace(exportName, $"{OperationEndsWith}(\\d*)$", "$1"), fileLocate);
      
      var extracted = operation.Export(exportName, fileLocate, Controller);
      operation.Priority = -1;
      
      return new KeyValueSnippet(
        new KeySnippet(method.ToUpper(), isFormat: false),
        extracted,
        Controller
      );
    }).ToList();

    var snippet = new KeyValuesSnippet(operations);

    snippet.AddComments(new List<(string, string?)>
    {
      (nameof(pathItemObject.Summary), pathItemObject.Summary),
      (nameof(pathItemObject.Description), pathItemObject.Description)
    });
    return snippet;
  }

  private (string, string) DecideOperationExtractInfo(string url, string method, OperationObject operationObject)
  {
    var fileName = operationObject.Tags.FirstOrDefault() ?? url.Split("/").ToList()
      .Find(tag => !string.IsNullOrWhiteSpace(tag) && !tag.Contains("{") && !tag.Contains("api"));
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


  private const string OperationEndsWith = "EP";

  private HashSet<string> OperationExportNames { get; } = new();


  public PathItemObjectHandler(Controller controller) : base(controller)
  {
  }
}