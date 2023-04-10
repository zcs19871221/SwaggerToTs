using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.Handlers;

public class ParameterObjectHandler: ReferenceObjectHandler
{
  

  public string GetKey(ParameterObject p)
  {
    var parameterObject = GetRefMaybe(p) ?? p;
    
    return parameterObject.Name + "_" + parameterObject.In;
    
  }

  public ParameterObjectHandler(Controller controller) : base(controller)
  {
  }

  public bool HasSchema(ParameterObject parameterObject)
  {
    var p = GetRefOrSelf(parameterObject);
    return p.Schema != null || p.Content?.Count > 0;
  }

  public ValueSnippet Generate(ParameterObject parameterObject, bool isForHeader = false)
  {
    return GetOrCreateThenSaveValue(parameterObject, p =>
    {
      var schema = p.Schema;
      var content = p.Content;
      var serializeFormat = p.Style;
      if (schema == null && content?.Count == 1)
      {
        schema = content.FirstOrDefault().Value.Schema;
        serializeFormat ??= content.FirstOrDefault().Key;
      }

      ValueSnippet snippet;
      
      if (isForHeader)
      {
        snippet = Controller.SchemaObjectHandlerWrapper.Construct(schema ?? throw new InvalidOperationException());
      }
      else
      {
        snippet = new KeyValueSnippet(new KeySnippet(p.Name, p.Required, true),
          Controller.SchemaObjectHandlerWrapper.Construct(schema ?? throw new InvalidOperationException()), Controller);
      }
      
      snippet.AddComments(new List<(string, string?)>
      {
        (nameof(p.Description), p.Description),
        (nameof(p.Deprecated), p.Deprecated ? "True" : ""),
        (nameof(p.AllowEmptyValue), p.AllowEmptyValue ? "True" : ""),
        ("SerializeFormat", serializeFormat),
        (nameof(p.Explode), p.Explode ? "True" : ""),
        (nameof(p.AllowReserved), p.AllowReserved ? "True" : ""),
      });
      return snippet;
    });
  }


}