namespace SwaggerToTs.Snippets;


public class ExportedValueSnippet: ValueSnippet
{
  public readonly ValueSnippet IsolateSnippet;

  public ExportedValueSnippet(ValueSnippet isolateSnippet,Controller controller)
  {
    IsolateSnippet = isolateSnippet;
    if (IsolateSnippet.IsNullable && IsolateSnippet.ExportType is ExportType.Enum)
    {
      IsNullable = true;
    }
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
    return (string.IsNullOrWhiteSpace(Generic) ? IsolateSnippet.ExportName : $"{Generic}<{IsolateSnippet.ExportName}>") ?? throw new InvalidOperationException();  }
}

