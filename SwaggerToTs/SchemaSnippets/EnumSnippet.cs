using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaSnippets;

public class EnumSnippet : SchemaSnippet
{

  private string contents = "";
  public EnumSnippet(SchemaObject schema, Options options) : base(schema)
  {
    if (options.Get<EnumUseEnum>().Value)
    {
      contents =  "{\n  " + string.Join(",\n  ", schema.Enum.Select(e =>  e + $" = '{e}'")) + "\n}";
      ExportType = ExportType.Enum;
    }
    else
    {
      ExportType = ExportType.Type;
      contents = string.Join(" | ", schema.Enum.Select(e =>
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

  public override string GenerateExportedContent(Options options, List<ValueSnippet> imports)
  {
    if (ExportType == ExportType.Enum)
    {
      return $"export const enum {ExportName} = {contents}";
    }

    return $"export type {ExportName} = {contents};";
  }

  public override string GenerateContent(Options options, List<ValueSnippet> imports)
  {
    return contents;
  }
}