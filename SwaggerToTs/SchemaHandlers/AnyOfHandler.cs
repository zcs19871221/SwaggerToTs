using SwaggerToTs.OpenAPIElements;

namespace SwaggerToTs.SchemaHandlers;

public class AnyOfHandler : ISchemaHandler
{
  public bool IsMatch(SchemaObject schema)
  {
    return schema.AnyOf.Any();
  }


  public void CreateTsCode(SchemaObject schema)
  {
    schema.SchemaType = SchemaTypeEnums.AnyOf;
    List<TsCodeElement> contents = new();
    foreach (var schemaObject in schema.AnyOf)
    {
      var each = schemaObject.GenerateTsCode();
      schema.Merge(each, element => { contents.Add(element); });
    }

    var name = TsCodeWriter.AnyOfName;

    schema.Contents = name + Helper.CreateHelperGeneric(contents.Select(e => e.GenerateCodeBody()));
    schema.ImportedHelpers.Add(name);
  }
}