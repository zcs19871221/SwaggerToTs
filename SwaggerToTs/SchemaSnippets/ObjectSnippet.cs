using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaSnippets;

public class ObjectSnippet:ValueSnippet
{
  private readonly ValuesSnippet _propertySnippet;
  public ObjectSnippet(ValuesSnippet propertySnippet)
  {
    _propertySnippet = propertySnippet;
  }

  public override string GenerateExportedContent( GeneratingInfo generatingInfo)
  {
    return $"export interface {ExportName} {AddBrackets(GenerateContent( generatingInfo))}";
  }

  public override string GenerateContent(GeneratingInfo generatingInfo)
  {
    return _propertySnippet.Generate(generatingInfo);
  }
}