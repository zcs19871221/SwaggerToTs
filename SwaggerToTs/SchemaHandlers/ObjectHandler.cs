using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.TypeScriptGenerator;

namespace SwaggerToTs.SchemaHandlers;

public class ObjectHandler : ISchemaHandler
{
  public bool IsMatch(SchemaObject schema)
  {
    return schema.Type == "object";
  }


  public static void Create(SchemaObject schema)
  {
    schema.AddComment(nameof(schema.MinProperties), schema.MinProperties.ToString())
      .AddComment(nameof(schema.MaxProperties), schema.MaxProperties.ToString());

    schema.ExportTypeValue = ExportType.Interface;
    schema.Merge(TsCodeElement.CreateFragment(schema.Properties, (key, o) =>
    {
      var wrapper = new TsCodeFragment
      {
        Name = TsCodeElement.ToCamelCase(key),
        ReadOnly = true,
        Optional = false,
      };
      var item = o.GenerateTsCode();
      wrapper.Optional = !schema.Required.Contains(key);
      return wrapper.Merge(item);
    }));
  }

  public void CreateTsCode(SchemaObject schema)
  {
    schema.SchemaType = SchemaTypeEnums.Object;
    Create(schema);
  }
}