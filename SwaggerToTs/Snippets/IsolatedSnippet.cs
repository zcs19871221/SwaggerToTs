
namespace SwaggerToTs.Snippets;

public abstract class IsolatedSnippet: CommonSnippet
{
  public ExportType ExportType { protected init; get; }
  public string? ExportName { protected set; get; }
  public string? FileLocate { protected set; get; }

  public List<CodeLocation?> AppearedLocations { get; } = new();

  protected bool IsIsolateSnippet()
  {
    return !string.IsNullOrWhiteSpace(ExportName);
  }

  protected string GenerateIsolate(GeneratingInfo generatingInfo)
  {
    var previous = generatingInfo.InWhichIsolateSnippet;
    
    generatingInfo.InWhichIsolateSnippet = this;
     
    var result = CreateComments() + GenerateIsolateContent(generatingInfo);
    generatingInfo.InWhichIsolateSnippet = previous;
    return result;

  }

  public bool IsAppearedOnlyInResponse(CodeLocation firstConstructLocation)
  {
    return AppearedLocations.Count switch
    {
      0 => throw new Exception("one isolate snippet must have at least one reference"),
      1 => firstConstructLocation == CodeLocation.Response,
      > 1 when AppearedLocations.Any(e => e == CodeLocation.Response) &&
               AppearedLocations.All(e => e != CodeLocation.Request) => true,
      _ => false
    };
  }
  
  protected abstract string GenerateIsolateContent(GeneratingInfo generatingInfo);

}

public enum ExportType
{
  Interface,
  Type,
  Enum
}
