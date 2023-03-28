using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.TypeScriptGenerator;

namespace SwaggerToTs.SchemaHandlers;

public class AllOfHandler : ISchemaHandler
{
  public bool IsMatch(SchemaObject schema)
  {
    return schema.Allof.Any();
  }


  public void CreateTsCode(SchemaObject schema)
  {
    schema.SchemaType = SchemaTypeEnums.AllOf;
    List<TsCodeElement> contents = new();
    List<string> exportNames = new();
    if (schema.Type == "object")
    {
      ObjectHandler.Create(schema);
    }
    foreach (var each in schema.Allof.Select(schemaObject => schemaObject.GenerateTsCode()))
    {
      schema.Merge(each, element => { contents.Add(element); });
      if (!string.IsNullOrWhiteSpace(each.ExportName))
      {
        exportNames.Add(each.ExportName);
      }
    }

    if (exportNames.Count == schema.Allof.Count && !string.IsNullOrEmpty(schema.ExportName))
    {
      schema.Extends.AddRange(exportNames);
      schema.ExportTypeValue = ExportType.Interface;
    }
    else
    {
      var allContent = contents.Select(Helper.AddBracketIfNeed).ToList();
      if (!string.IsNullOrWhiteSpace(schema.GenerateCodeBody()))
      {
        allContent.Add(schema.GenerateCodeBody());
      }
      schema.Contents = string.Join(" & ", allContent);
    }
  }
}