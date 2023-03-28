namespace SwaggerToTs.OpenApiDocument;

public class OneOfHandler : ISchemaHandler
{
  public bool IsMatch(SchemaObject schema)
  {
    return schema.Oneof.Any();
  }

  public void CreateTsCode(SchemaObject schema)
  {
    schema.SchemaType = SchemaTypeEnums.OneOf;
    List<TsCodeElement> contents = new();
    foreach (var schemaObject in schema.Oneof)
    {
      var each = schemaObject.GenerateTsCode();
      schema.Merge(each, element => { contents.Add(element); });
    }


    var name = TsCodeWriter.OneOfName;

    schema.Contents = name + Helper.CreateHelperGeneric(contents.Select(e => e.GenerateCodeBody()));
    schema.ImportedHelpers.Add(name);
  }
}