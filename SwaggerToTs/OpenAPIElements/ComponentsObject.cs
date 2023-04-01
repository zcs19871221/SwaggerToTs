using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace SwaggerToTs.OpenAPIElements;

public class ComponentsObject
{
  public Dictionary<string, SchemaObject>? Schemas { get; set; }

  public Dictionary<string, ResponseObject>? Responses { get; set; }

  public Dictionary<string, ParameterObject>? Parameters { get; set; }

  public Dictionary<string, RequestBodyObject>? RequestBodies { get; set; }

  public Dictionary<string, HeaderObject>? Headers { get; set; }

  [OnDeserialized]
  public void OnDeserialized(StreamingContext _)
  {
    CreateNameAccordingReference(Schemas);
    CreateNameAccordingReference(Responses);
    CreateNameAccordingReference(Parameters);
    CreateNameAccordingReference(RequestBodies);
    CreateNameAccordingReference(Headers);
  }

  private void CreateNameAccordingReference<T>(Dictionary<string, T>? schemas)
    where T : TsCodeElement
  {
    if (schemas == null) return;

    Dictionary<string, List<(string, TsCodeElement)>> shortMappingRefs = new();
    List<(string, T)> genericNames = new();
    List<(string, T)> inlineTypeNames = new();
    foreach (var (reference, schemaObject) in schemas)
    {
      var formatName = reference;
      if (reference.Contains("[["))
      {
        genericNames.Add((reference, schemaObject));
        continue;
      }

      if (reference.Contains("+"))
      {
        inlineTypeNames.Add((reference, schemaObject));
        continue;
      }

      if (reference.Contains('-')) formatName = formatName.Replace("-", "");
      if (Regex.IsMatch(formatName, @"[^a-zA-Z_\d$.]")) throw new Exception($"can' get export name from {formatName}");

      var shortName = formatName.Split(".")[^1];
      shortMappingRefs.GetOrCreate(shortName).Add((formatName, schemaObject));
    }

    foreach (var (shortName, references) in shortMappingRefs)
      if (references.Count > 1)
      {
        var longNames = references.Select(e => e.Item1.Split(".").ToHashSet()).ToList();
        var firstValue = new List<string>(longNames.First());
        for (var i = 0; i < firstValue.Count; i++)
        {
          var s = firstValue[firstValue.Count - i - 1];
          if (longNames.Count(e => e.Contains(s)) > 1)
            foreach (var longName in longNames)
              longName.Remove(s);
        }


        for (var i = 0; i < references.Count; i++)
          references[i].Item2.ExportName = longNames[i].LastOrDefault("") + shortName;
      }
      else
      {
        references.First().Item2.ExportName = shortName;
      }

    foreach (var (reference, schemaObject) in genericNames)
    {
      var matched = Regex.Match(reference, @"\.([^.`]+)`\d+\[\[([^]]+)\]\]");
      var baseClass = matched.Groups[1].ToString();
      var baseClassSchema = schemas.GetValueOrDefault(baseClass);
      var baseClassShotName = baseClassSchema?.ExportName ?? baseClass.Split(".")[^1];
      var genericShortNames = string.Join("$", matched.Groups[2].ToString().Split(",")
        .Where(e => !e.Contains('=') && schemas.ContainsKey(e)).Select(e =>
          schemas.GetValueOrDefault(e)?.ExportName ?? e.Split(",")[^1]));
      schemaObject.ExportName = baseClassShotName + genericShortNames;
    }

    foreach (var (reference, schemaObject) in inlineTypeNames)
      schemaObject.ExportName = string.Join("At",
        reference.Split("+").Reverse().Select(e => schemas.GetValueOrDefault(e)?.ExportName ?? e.Split(".")[^1]));
  }


  private T TryGet<T>(Dictionary<string, T>? element, string key)
    where T : TsCodeElement
  {
    if (element == null) throw new Exception($"can't find ref: {key}");

    if (!element.TryGetValue(key, out var target)) throw new Exception($"can't find ref: {key}");

    return target;
  }

  
}