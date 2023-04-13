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
    IsolateSnippet.AppearedLocations.Add(controller.CurrentLocation);
    Dependencies.Add(isolateSnippet);
    controller.IsolateSnippets.Add(isolateSnippet);
  }


  protected override string GenerateIsolateContent(GeneratingInfo generatingInfo)
  {
    throw new Exception("extracted snippet should not export again");
  }

  protected override string GenerateContent(GeneratingInfo generatingInfo)
  {
    return IsolateSnippet.ExportName ?? throw new InvalidOperationException();  
  }
}

