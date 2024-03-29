using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaSnippets;

public class ArraySnippet : ValueSnippet
{

  private readonly ValueSnippet _item;
  public ArraySnippet(ValueSnippet item)
  {
    ExportType = ExportType.Type;
    IsReadOnly = true;
    _item = item;
    AddComments(item.Comments!);
    item.Comments.Clear();
  }

  protected override string GenerateIsolateContent(GeneratingInfo generatingInfo)
  {
    return $"export type {ExportName} = {GenerateContent(generatingInfo)};";
  }

  protected override string GenerateContent(GeneratingInfo generatingInfo)
  {
    var itemContent = _item.Generate(generatingInfo);
    if (_item.IsNullable)
    {
      itemContent += " | null";
    }
    if (_item is EnumSnippet or OneOfSnippet or AllOfSnippet or AnyOfSnippet || _item.IsNullable)
    {
      itemContent = $"({itemContent})";
    }

    return itemContent + "[]";
  }
}