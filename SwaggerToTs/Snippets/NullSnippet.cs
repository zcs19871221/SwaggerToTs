namespace SwaggerToTs.Snippets;


public class NullSnippet: ValueSnippet
{
  protected override string GenerateIsolateContent(GeneratingInfo generatingInfo)
  {
    throw new NotImplementedException();
  }

  protected override string GenerateContent(GeneratingInfo generatingInfo)
  {
    return "null";
  }
  

}

