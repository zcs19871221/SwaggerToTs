using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaSnippets;

public class RecordObjectSnippet : ValueSnippet
{
  public RecordObjectSnippet() 
  {

    ExportType = ExportType.Type;
  }

  protected override string GenerateIsolateContent(GeneratingInfo generatingInfo)
  {
    return $"export type {ExportName} = {GenerateContent(generatingInfo)}";
  }

  protected override string GenerateContent(GeneratingInfo generatingInfo)
  {
    return @"Record<string, unknown>";  
  }
}