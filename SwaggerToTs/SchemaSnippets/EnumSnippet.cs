using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaSnippets;

public class EnumSnippet : SchemaSnippet
{

  private readonly IEnumerable<object> _enums;

  public EnumSnippet(SchemaObject schema) : base(schema)
  {
    _enums = schema.Enum;
  }

  public override string GenerateExportedContent(Options options,  GeneratingInfo generatingInfo)
  {
    var contents = GenerateContent(options, generatingInfo);
    return ExportType == ExportType.Enum ? $"export const enum {ExportName} = {contents}" : $"export type {ExportName} = {contents};";
  }

  public override string GenerateContent(Options options, GeneratingInfo generatingInfo)
  {
    if (ExportType == ExportType.Enum)
    {
      return  "{\n  " + string.Join(",\n  ", _enums.Select(e =>  e + $" = '{e}'")) + "\n}";
    }
    if (ExportType == ExportType.Type)
    {
      return string.Join(" | ", _enums.Select(e =>
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

    throw new Exception($"enum can't handle export type: {ExportType.ToString()}");
  }
}