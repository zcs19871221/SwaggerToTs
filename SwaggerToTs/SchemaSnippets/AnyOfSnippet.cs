using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaSnippets;

public class AnyOfSnippet : ValueSnippet
{
  private readonly List<ValueSnippet> _anyOfs;

  public AnyOfSnippet(List<ValueSnippet> anyOfs)
  {
    _anyOfs = anyOfs;
  }

  public override string GenerateExportedContent(GeneratingInfo generatingInfo)
  {
    return $"export type {ExportName} = {GenerateContent(generatingInfo)}";
  }

  public override string GenerateContent(GeneratingInfo generatingInfo)
  {
    generatingInfo.AddHelper(Controller.AnyOfName);
    return $"{Controller.AnyOfName}<{string.Join(NewLine, _anyOfs.Select(e => e.Generate(generatingInfo)))}>";  }
}