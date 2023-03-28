using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.TypeScriptGenerator;

namespace SwaggerToTs.SchemaHandlers;

public class ArrayHandler : ISchemaHandler
{
  public bool IsMatch(SchemaObject schema)
  {
    return schema.Type == "array";
  }


  public void CreateTsCode(SchemaObject schema)
  {
    schema.SchemaType = SchemaTypeEnums.Array;
    schema.ExportTypeValue = ExportType.Type;
    schema.AddComment(nameof(schema.MaxItems), schema.MaxItems.ToString())
      .AddComment(nameof(schema.MinItems), schema.MinItems.ToString()).AddComment(nameof(schema.UniqueItems),
        schema.UniqueItems?.ToString()).ReadOnly = true;
    if (schema.Items == null) throw new Exception("array should not have empty items");

    schema.Optional ??= schema.Items.Optional;
    schema.Merge(schema.Items.GenerateTsCode(), element =>
    {
      var content = Helper.AddBracketIfNeed(element);
      schema.Contents += content + "[]";
    });
  }
}