using System.Text.RegularExpressions;
using SwaggerToTs.TypeScriptGenerator;

namespace SwaggerToTs.OpenAPIElements;

public class ResponseObject : TsCodeElement
{
  public OperationObject? OperationObject { get; set; }
  public Dictionary<string, HeaderObject>? Headers { get; set; }

  public Dictionary<string, MediaTypeObject>? Content { get; set; }
  public string StatusCode { get; set; }

  protected override void ValidateOpenApiDocument()
  {
    if (!string.IsNullOrWhiteSpace(Reference)) return;
    if (string.IsNullOrEmpty(Description)) throw new Exception("Response Description should not be null or empty");
  }

  protected override TsCodeElement CreateTsCode()
  {
    Dictionary<string, TsCodeElement> response = new();
    if (Headers != null)
      response.Add("Headers",
        CreateFragment(Headers, (key, item) =>
        {
          var header = item.GenerateTsCode();
          var wrapper = new TsCodeFragment()
          {
            Name = key,
            Optional = header.Optional
          };
          return wrapper.Merge(header);
        }));

    if (Content != null)
    {
      var content = CreateFragment(dict: Content,  (contentType, item) =>
      {
        var wrapper = new TsCodeFragment
        {
          Name = contentType,
          Optional = false,
        };
        var schema = item.GenerateTsCode();
        var contentTypeName =
          ToPascalCase(Regex.Replace(contentType,
            @"[^a-zA-Z_\d$](\S)", m => m.Groups[1].ToString().ToUpper()));
        return wrapper.Merge(schema);
        // var extractedResponseName = "";
        // var fileLocate = "";
        // if (!string.IsNullOrWhiteSpace(ExportName))
        // {
        //   extractedResponseName = ExportName + contentTypeName;
        //   fileLocate = FileLocate;
        // }
        // else if (OperationObject != null)
        // {
        //
        //   extractedResponseName = OperationObject.ExportName?.Replace(OperationObject.EndWith, "") + StatusCode + contentTypeName;
        //   fileLocate = OperationObject.FileLocate;
        // }
        //   
        // schema.ExtractTo(extractedResponseName, fileLocate);
        //
        // return wrapper.Merge(schema);
      });
      response.Add("Content", content);
    }
    else
    {
      var content = new TsCodeFragment
      {
        Contents = "null",
        Optional = false
      };
      response.Add("Content", content);
    }
  
    return Merge(CreateFragment(dict:response));
  }
}