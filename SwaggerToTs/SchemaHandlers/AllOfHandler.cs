using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaHandlers;

public class AllOfHandler : ISchemaHandler
{
  public bool IsMatch(SchemaObject schema)
  {
    return schema.Allof.Any();
  }


  public override string ToString()
  {
    return base.ToString();
  }

  public void Aggregate(SchemaObject schema)
  {
    schema.SchemaType = SchemaTypeEnums.AllOf;
    List<TsCodeElement> contents = new();
    List<string> exportNames = new();
    if (ALlOf every is Extracted)
    {
      var x = new ValueSnippet();
      var isolate = x.RefactorAndSave();
      isolate.extends.Add(xxx);
      return isolate;
    } else {
      return this;
    }
    // if (schema.Type == "object")
    // {
    //   KeyValueSnippet objectT;
    // }
    // foreach (var each in schema.Allof.Select(schemaObject => schemaObject.Aggregate()))
    // {
    //   if (each.)
    //   schema.Merge(each, element => { contents.Add(element); });
    //   if (!string.IsNullOrWhiteSpace(each.ExportName))
    //   {
    //     exportNames.Add(each.ExportName);
    //   }
    // }
    //
    // if (exportNames.Count == schema.Allof.Count && !string.IsNullOrEmpty(schema.ExportName))
    // {
    //   schema.Extends.AddRange(exportNames);
    //   schema.ExportTypeValue = ExportType.Interface;
    // }
    // else
    // {
    //   var allContent = contents.Select(Helper.AddBracketIfNeed).ToList();
    //   if (!string.IsNullOrWhiteSpace(schema.GenerateCodeBody()))
    //   {
    //     allContent.Add(schema.GenerateCodeBody());
    //   }
    //   schema.Contents = string.Join(" & ", allContent);
    // }
  }
}