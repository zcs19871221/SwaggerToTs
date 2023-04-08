using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.Handlers;

public class OperationObjectHandler : Handler
{
  public ValueSnippet Generate(OperationObject operationObject)
  {
    var parameterObjectHandler = Controller.ParameterObjectHandler;
    var requestBodyObjectHandler = Controller.RequestBodyObjectHandler;
    var responseObjectHandler = Controller.ResponseObjectHandler;
    operationObject.Parameters =
      operationObject.Parameters.Where(e => (parameterObjectHandler.GetRefMaybe(e) ?? e).Schema != null).ToList();
    var groupedParameters = operationObject.Parameters.GroupBy(e => (parameterObjectHandler.GetRefMaybe(e) ?? e).In, (key, g) => (key, g));
    var requestContent = groupedParameters.Where(e => e.g.Any()).Select(group =>
    {
      var parameterType = new KeySnippet(group.key ?? throw new InvalidOperationException());
      var parameters = new ValuesSnippet(group.g.Select(p => parameterObjectHandler.Generate(p)));

      return new KeyValueSnippet(parameterType, parameters, Controller) as ValueSnippet;
    }).ToList();
      

    if (operationObject.RequestBody != null)
      requestContent.Add(requestBodyObjectHandler.Generate(operationObject.RequestBody));


    var request = new KeyValueSnippet(new KeySnippet("Request"), new ValuesSnippet(requestContent), Controller);
    var responseContent =
      new List<KeyValueSnippet>(operationObject.Responses
        .Select(e => new KeyValueSnippet(new KeySnippet(e.Key), responseObjectHandler.Generate(e.Value), Controller)));


    var response = new KeyValueSnippet(new KeySnippet("Response"),new ValuesSnippet(responseContent), Controller);

    var snippet = new ValuesSnippet(new List<KeyValueSnippet>
    {
      request,
      response
    });
    snippet.AddComments(new List<(string, string?)>
    {
      (nameof(operationObject.Description), operationObject.Description),
      (nameof(operationObject.Summary), operationObject.Summary),
      (nameof(operationObject.Deprecated), operationObject.Deprecated.ToString())
    });
    return snippet;
  }

  public OperationObjectHandler(Controller controller) : base(controller)
  {
  }
}