namespace SwaggerToTs.Snippets;


public class NullSnippet: ValueSnippet
{
  public override string GenerateExportedContent(GeneratingInfo generatingInfo)
  {
    throw new NotImplementedException();
  }

  public override string GenerateContent(GeneratingInfo generatingInfo)
  {
    return "null";
  }
  

}

