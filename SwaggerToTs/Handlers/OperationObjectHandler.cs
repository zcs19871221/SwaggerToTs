using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.Handlers;

public class OperationObjectHandler : Handler
{
  public KeyValueSnippet Generate(OperationObject operationObject)
  {
    var parameterObjectHandler = Controller.ParameterObjectHandler;
    var requestBodyObjectHandler = Controller.RequestBodyObjectHandler;
    var responseObjectHandler = Controller.ResponseObjectHandler;
    var groupedParameters = operationObject.Parameters.GroupBy(e => (parameterObjectHandler.GetRefMaybe(e) ?? e).In, (key, g) => (key, g));
    var requestContent = groupedParameters.Where(e => e.g.Any()).Select(group =>
      new KeyValueSnippet(new KeySnippet(group.key ?? throw new InvalidOperationException()),
        new KeyValueSnippet(group.g.Select(p => parameterObjectHandler.Generate(p))))).ToList();

    if (operationObject.RequestBody != null)
      requestContent.Add(requestBodyObjectHandler.Generate(operationObject.RequestBody));


    var request = new KeyValueSnippet(new KeySnippet("Request"), new KeyValueSnippet(requestContent));
    var responseContent =
      new List<KeyValueSnippet>(operationObject.Responses
        .Select(e => new KeyValueSnippet(new KeySnippet(e.Key), responseObjectHandler.Generate(e.Value))));


    var response = new KeyValueSnippet(new KeySnippet("Response"), new KeyValueSnippet(responseContent));

    var snippet = new KeyValueSnippet(new List<KeyValueSnippet>
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