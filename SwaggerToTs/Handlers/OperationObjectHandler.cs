using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.Handlers;

public class OperationObjectHandler : Handler
{
  public ValueSnippet Generate(OperationObject operationObject, string exportName, string fileLocate)
  {
    var parameterObjectHandler = Controller.ParameterObjectHandler;
    var requestBodyObjectHandler = Controller.RequestBodyObjectHandler;
    var responseObjectHandler = Controller.ResponseObjectHandler;
    operationObject.Parameters =
      operationObject.Parameters.Where(e => (parameterObjectHandler.GetRefMaybe(e) ?? e).Schema != null).ToList();
    var groupedParameters = operationObject.Parameters.GroupBy(e => (parameterObjectHandler.GetRefMaybe(e) ?? e).In, (key, g) => (key, g));
    var requestContent = groupedParameters.Where(e => e.g.Any()).Select(group =>
    {
      var parameterTypeName = ToPascalCase(group.key ?? throw new InvalidOperationException());
      var parameterType = new KeySnippet(parameterTypeName, isFormat:false);
      var fields = group.g.Select(p => parameterObjectHandler.Generate(p)).ToList();
      ValueSnippet parameterToUse;
      if (fields.Count == 1)
      {
        parameterToUse = fields.First();
      }
      else
      {
        parameterToUse = new ValuesSnippet(fields);
      }
      
      if (!Controller.Options.Get<InlineRequest>().Value)
      {
        parameterToUse = parameterToUse.Export(exportName + parameterTypeName, fileLocate, Controller);
      }
      return new KeyValueSnippet(parameterType, parameterToUse, Controller) as ValueSnippet;
    }).ToList();
      

    if (operationObject.RequestBody != null)
      requestContent.Add(requestBodyObjectHandler.Generate(operationObject.RequestBody));

    var contents = new List<KeyValueSnippet>();
    if (requestContent.Count > 0)
    {
      contents.Add(new KeyValueSnippet(new KeySnippet("Request", isFormat:false), new ValuesSnippet(requestContent), Controller));
      
    }

    var responseContent =
      new List<KeyValueSnippet>(operationObject.Responses
        .Select(e => new KeyValueSnippet(new KeySnippet(e.Key, isFormat:false), responseObjectHandler.Generate(e.Value), Controller)));


    var response = new KeyValueSnippet(new KeySnippet("Responses", isFormat:false),new ValuesSnippet(responseContent), Controller);
    contents.Add(response);

    var snippet = new ValuesSnippet(contents);
    snippet.AddComments(new List<(string, string?)>
    {
      (nameof(operationObject.Summary), operationObject.Summary),
      (nameof(operationObject.Description), operationObject.Description),
      (nameof(operationObject.Deprecated), operationObject.Deprecated.ToString())
    });
    return snippet;
  }

  public OperationObjectHandler(Controller controller) : base(controller)
  {
  }
}