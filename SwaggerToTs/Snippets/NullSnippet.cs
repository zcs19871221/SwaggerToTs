namespace SwaggerToTs.Snippets;


public class NullSnippet: ValueSnippet
{
  public override string GenerateExportedContent(Options options, GeneratingInfo generatingInfo)
  {
    throw new NotImplementedException();
  }

  public override string GenerateContent(Options options, GeneratingInfo generatingInfo)
  {
    return "null";
  }
  

}

