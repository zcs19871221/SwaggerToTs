namespace SwaggerToTs.Snippets;


public class ExportedValueSnippet: ValueSnippet
{
  public readonly ValueSnippet IsolateSnippet;

  public ExportedValueSnippet(ValueSnippet isolateSnippet,Controller controller)
  {
    IsolateSnippet = isolateSnippet;
    if (IsolateSnippet.IsNullable && IsolateSnippet.ExportType is ExportType.Type or ExportType.Enum)
    {
      IsNullable = true;
    }
    IsolateSnippet.UsedBy.Add(this);
    controller.IsolateSnippets.Add(isolateSnippet);
  }

  public string? Generic { get; set; }
  
  public override string GenerateExportedContent(Options options, List<ValueSnippet> imports)
  {
    throw new Exception("extracted snippet should not export again");
  }

  public override string GenerateContent(Options options, List<ValueSnippet> imports)
  {
    return (string.IsNullOrWhiteSpace(Generic) ? IsolateSnippet.ExportName : $"{Generic}<{IsolateSnippet.ExportName}>") ?? throw new InvalidOperationException();  }
}

