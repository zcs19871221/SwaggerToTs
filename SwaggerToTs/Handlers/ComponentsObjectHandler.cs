using System.Text.RegularExpressions;
using SwaggerToTs.OpenAPIElements;

namespace SwaggerToTs.Handlers;

public class ComponentsObjectHandler : Handler
{

  public Dictionary<string,string> Construct(ComponentsObject? componentsObject)
  {
    if (componentsObject == null)
    {
      return new Dictionary<string, string>();
    }
    var referenceUrls = new List<string>();
    Add(componentsObject.Headers, referenceUrls);
    Add(componentsObject.Parameters, referenceUrls);
    Add(componentsObject.Responses, referenceUrls);
    Add(componentsObject.Schemas, referenceUrls);
    Add(componentsObject.RequestBodies, referenceUrls);
    
    var referenceMappingShortName = new Dictionary<string, string>();
    Dictionary<string, List<string>> shortMappingRefs = new();
    List<string> genericNames = new();
    List<string> inlineTypeNames = new();
    foreach (var reference in referenceUrls)
    {
      var formatName = reference;
      if (reference.Contains("[["))
      {
        genericNames.Add(reference);
        continue;
      }

      if (reference.Contains("+"))
      {
        inlineTypeNames.Add(reference);
        continue;
      }

      if (reference.Contains('-')) formatName = formatName.Replace("-", "");
      if (Regex.IsMatch(formatName, @"[^a-zA-Z_\d$.]")) throw new Exception($"can' get export name from {formatName}");

      var shortName = formatName.Split(".")[^1];
      shortMappingRefs.GetOrCreate(shortName).Add(formatName);
    }

    foreach (var (shortName, references) in shortMappingRefs)
      if (references.Count > 1)
      {
        var longNames = references.Select(e => e.Split(".").ToHashSet()).ToList();
        var firstValue = new List<string>(longNames.First());
        for (var i = 0; i < firstValue.Count; i++)
        {
          var s = firstValue[firstValue.Count - i - 1];
          if (longNames.Count(e => e.Contains(s)) > 1)
            foreach (var longName in longNames)
              longName.Remove(s);
        }


        for (var i = 0; i < references.Count; i++)
        {
          referenceMappingShortName.Add(references[i], longNames[i].LastOrDefault("") + shortName);
        }
      }
      else
      {
        referenceMappingShortName.Add(references.FirstOrDefault() ?? throw new InvalidOperationException(), shortName);
      }

    foreach (var reference in genericNames)
    {
      var matched = Regex.Match(reference, @"\.([^.`]+)`\d+\[\[([^]]+)\]\]");
      var baseClass = matched.Groups[1].ToString();
      var baseClassSchema = referenceMappingShortName.GetValueOrDefault(baseClass);
      var baseClassShotName = baseClassSchema ?? baseClass.Split(".")[^1];
      var genericShortNames = string.Join("$", matched.Groups[2].ToString().Split(",")
        .Where(e => !e.Contains('=') && referenceMappingShortName.ContainsKey(e)).Select(e =>
          referenceMappingShortName.GetValueOrDefault(e) ?? e.Split(",")[^1]));
      referenceMappingShortName.Add(reference, baseClassShotName + genericShortNames);
    }

    foreach (var reference in inlineTypeNames)
    {
      referenceMappingShortName.Add(reference, string.Join("At",
        reference.Split("+").Reverse().Select(e => referenceMappingShortName.GetValueOrDefault(e) ?? e.Split(".")[^1])));
    }

    return referenceMappingShortName;
  }


  
  private void Add<T>(Dictionary<string, T>? dict, List<string> allDict) where T:ReferenceObject
  {
    if (dict == null)
    {
      return;
    }
    allDict.AddRange(dict.Keys);

  }

  
  public ComponentsObjectHandler(Controller controller) : base(controller)
  {
  }
  
}