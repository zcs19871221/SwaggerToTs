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

  public WrapperSnippet Generate(ParameterObject parameterObject)
  {
    return Handle(parameterObject, p =>
    {
      var name = p.Name;
      return CreateWrapperSnippet(p, name);
    });
  }

  public WrapperSnippet CreateWrapperSnippet(ParameterObject p, string name)
  {
    var required = p.Required;
    var schema = p.Schema;
    var content = p.Content;
    var serializeFormat = p.Style;
    if (schema == null && content?.Count == 1)
    {
      var (mediaType, contentSchema) = content.FirstOrDefault();
      serializeFormat = mediaType;
      schema = contentSchema.Schema;
    }

    var keySnippet = new KeySnippet(name ?? throw new InvalidOperationException(), required);
    var snippet = WrapperSnippet.Create(keySnippet,
      Controller.SchemaObjectHandler.Generate(schema ?? throw new InvalidOperationException()));
    snippet.AddComments(new List<(string, string?)>
    {
      (nameof(p.Description), p.Description),
      (nameof(p.Deprecated), p.Deprecated ? "true" : ""),
      (nameof(p.AllowEmptyValue), p.AllowEmptyValue ? "true" : ""),
      ("SerializeFormat", serializeFormat),
      (nameof(p.Explode), p.Explode ? "true" : ""),
      (nameof(p.AllowReserved), p.AllowReserved ? "true" : ""),
    });
    return snippet;
  }
}