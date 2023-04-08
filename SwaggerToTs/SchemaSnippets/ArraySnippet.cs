using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaSnippets;

public class ArraySnippet : SchemaSnippet
{

  private ValueSnippet _item;
  public ArraySnippet(SchemaObject schema, Controller controller) : base(schema)
  {
    ExportType = ExportType.Type;
    AddComments(new []
    {
      (nameof(schema.MaxItems), schema.MaxItems.ToString()),
      (nameof(schema.MinItems), schema.MinItems.ToString()),
      (nameof(schema.UniqueItems), schema.UniqueItems?.ToString())
    });
      IsReadOnly = true;
      _item = controller.SchemaObjectHandlerWrapper.Construct(schema.Items ??
                                                                       throw new InvalidOperationException());

  }

  public override string GenerateExportedContent(Options options, GeneratingInfo generatingInfo)
  {
    return $"export type {ExportName} = {GenerateContent(options, generatingInfo)}";
  }

  public override string GenerateContent(Options options, GeneratingInfo generatingInfo)
  {
    var itemContent = _item.Generate(options, generatingInfo);
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