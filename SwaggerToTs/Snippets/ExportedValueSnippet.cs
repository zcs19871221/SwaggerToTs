namespace SwaggerToTs.Snippets;


public class ExportedValueSnippet: ValueSnippet
{
  private readonly ValueSnippet _isolateSnippet;

  public ExportedValueSnippet(ValueSnippet isolateSnippet,Controller controller)
  {
    _isolateSnippet = isolateSnippet;
    if (_isolateSnippet.IsNullable && _isolateSnippet.ExportType is ExportType.Type or ExportType.Enum)
    {
      IsNullable = true;
    }
    _isolateSnippet.UsedBy.Add(this);
    Dependencies.Add(isolateSnippet);
    controller.IsolateSnippets.Add(isolateSnippet);
  }

  public string? Generic { get; set; }

  protected override string GenerateExportedContent(GeneratingInfo generatingInfo)
  {
    throw new Exception("extracted snippet should not export again");
  }

  protected override string GenerateContent(GeneratingInfo generatingInfo)
  {
    return (string.IsNullOrWhiteSpace(Generic) ? _isolateSnippet.ExportName : $"{Generic}<{_isolateSnippet.ExportName}>") ?? throw new InvalidOperationException();  }
}

