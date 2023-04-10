using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaSnippets;

public class OneOfSnippet : ValueSnippet
{
  private readonly List<ValueSnippet> _oneOfs;

  public OneOfSnippet(List<ValueSnippet> oneOfs)
  {
    ExportType = ExportType.Type;
    _oneOfs =oneOfs;
  }

  public override string GenerateExportedContent(GeneratingInfo generatingInfo)
  {
    return $"export type {ExportName} = {GenerateContent(generatingInfo)}";
  }

  public override string GenerateContent(GeneratingInfo generatingInfo)
  {
    generatingInfo.AddHelper(Controller.OneOfName);
    return $"{Controller.OneOfName}<{string.Join(NewLine, _oneOfs.Select(e => e.Generate(generatingInfo)))}>";  }
}