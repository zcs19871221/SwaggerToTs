using SwaggerToTs.OpenAPIElements;

namespace SwaggerToTs.SchemaHandlers;


public interface ISchemaHandler
{
  public bool IsMatch(SchemaObject schema);
  public void CreateTsCode(SchemaObject schema);
}

public static class Helper
{
  public static string AddBracketIfNeed(TsCodeElement element)
  {
    var content = element.GenerateCodeBody();
    if (element.ExportName == null &&
        element is SchemaObject { SchemaType: SchemaTypeEnums.Enum } or SchemaObject { Nullable: true }
          or SchemaObject { SchemaType: SchemaTypeEnums.OneOf } or SchemaObject { SchemaType: SchemaTypeEnums.AllOf }
          or SchemaObject { SchemaType: SchemaTypeEnums.AnyOf })
      content = $"({content})";

    return content;
  }

  public static string CreateHelperGeneric(IEnumerable<string> values)
  {
    return $"<[{string.Join(", ", values)}]>";
  }
}
