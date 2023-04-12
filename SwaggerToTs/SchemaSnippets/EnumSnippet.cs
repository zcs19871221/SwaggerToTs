using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaSnippets;

public class EnumSnippet : ValueSnippet
{

  private readonly IEnumerable<object> _enums;

  public EnumSnippet(IEnumerable<object> enums, ExportType exportType) 
  {
    _enums = enums;
    ExportType = exportType;
  }

  protected override string GenerateIsolateContent( GeneratingInfo generatingInfo)
  {
    var contents = GenerateContent(generatingInfo);
    return ExportType == ExportType.Enum ? $"export const enum {ExportName} {AddBrackets(contents)}" : $"export type {ExportName} = {contents};";
  }

  protected override string GenerateContent(GeneratingInfo generatingInfo)
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