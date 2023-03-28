using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.TypeScriptGenerator;

namespace SwaggerToTs.SchemaHandlers;

public class EnumHandler : ISchemaHandler
{
  public bool IsMatch(SchemaObject schema)
  {
    return schema.Enum.Any();
  }

  public void CreateTsCode(SchemaObject schema)
  {
    schema.SchemaType = SchemaTypeEnums.Enum;
  
    if (TsCodeWriter.Get().Options.Get<EnumUseEnum>().Value)
    {
      schema.Contents =  "{\n  " + string.Join(",\n  ", schema.Enum.Select(e =>  e + $" = '{e}'")) + "\n}";
      schema.ExportTypeValue = ExportType.Enum;
    }
    else
    {
      schema.ExportTypeValue = ExportType.Type;
      schema.Contents = string.Join(" | ", schema.Enum.Select(e =>
      {
        switch (e)
        {
          case byte:
          case sbyte:
          case ushort:
          case uint:
          case ulong:
          case short:
          case int:
          case long:
          case decimal:
          case double:
          case float:
            return $"{e}";
          default:
            return $"'{e}'";
        }
      }));
    }
 
  }
}