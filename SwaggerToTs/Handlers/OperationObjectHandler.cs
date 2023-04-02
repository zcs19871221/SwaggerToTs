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
      KeyValueSnippet.Create(new KeySnippet(group.key ?? throw new InvalidOperationException()),
        KeyValueSnippet.Create(group.g.Select(p => parameterObjectHandler.Generate(p))))).ToList();

    if (operationObject.RequestBody != null)
      requestContent.Add(requestBodyObjectHandler.Generate(operationObject.RequestBody));


    var request = KeyValueSnippet.Create(new KeySnippet("Request"), KeyValueSnippet.Create(requestContent));
    var responseContent =
      new List<KeyValueSnippet>(operationObject.Responses
        .Select(e => KeyValueSnippet.Create(new KeySnippet(e.Key), responseObjectHandler.Generate(e.Value))));


    var response = KeyValueSnippet.Create(new KeySnippet("Response"),responseContent);

    var snippet = KeyValueSnippet.Create(new List<KeyValueSnippet>
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