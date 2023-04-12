using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.SchemaSnippets;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.Handlers;

public class OperationObjectHandler : Handler
{
  public ValueSnippet Construct(OperationObject operationObject, string exportName, string fileLocate)
  {
    var contents = new List<KeyValueSnippet>();

    Controller.CurrentLocation = CodeLocation.Request;
    ConstructRequest(operationObject, exportName, fileLocate, contents);

    Controller.CurrentLocation = CodeLocation.Response;
    var response = Controller.ResponseObjectHandler.Generate(operationObject.Responses);

    Controller.CurrentLocation = CodeLocation.Response;
    contents.Add(response);
    var snippet = new KeyValuesSnippet(contents);
    snippet.AddComments(new List<(string, string?)>
    {
      (nameof(operationObject.Summary), operationObject.Summary),
      (nameof(operationObject.Description), operationObject.Description),
      (nameof(operationObject.Deprecated), operationObject.Deprecated.ToString())
    });
    return snippet;
  }

  private void ConstructRequest(OperationObject operationObject, string exportName, string fileLocate,
    List<KeyValueSnippet> contents)
  {
    var parameterObjectHandler = Controller.ParameterObjectHandler;
    var requestBodyObjectHandler = Controller.RequestBodyObjectHandler;

    operationObject.Parameters =
      operationObject.Parameters.Where(parameterObjectHandler.HasSchema).ToList();
    var parametersGroupedByType =
      operationObject.Parameters.GroupBy(e => parameterObjectHandler.GetRefOrSelf(e).In, (key, g) => (key, g));
    var parameterContents = parametersGroupedByType.Where(e => e.g.Any()).Select(group =>
    {
      var parameterTypeName = ToPascalCase(group.key ?? throw new InvalidOperationException());
      var groupedParametersRequired = group.g.Any(p => parameterObjectHandler.GetRefOrSelf(p).Required);
      var parameterFields = group.g.Select(p => parameterObjectHandler.Generate(p)).ToList();
      var parameterSet = parameterFields.Count == 1 ? parameterFields.First() : new AllOfSnippet(parameterFields);

      if (!Controller.Options.Get<InlineRequest>().Value && parameterSet is not ExportedValueSnippet)
        parameterSet = parameterSet.Export(exportName + parameterTypeName, fileLocate, Controller);

      return new KeyValueSnippet(new KeySnippet(parameterTypeName, groupedParametersRequired, isFormat: false),
        parameterSet, Controller);
    }).ToList();

    if (operationObject.RequestBody != null)
    {
      var body = requestBodyObjectHandler.Generate(operationObject.RequestBody);
      if (body != null) parameterContents.Add(body);
    }

    if (parameterContents.Count > 0)
      contents.Add(new KeyValueSnippet(new KeySnippet("Request", isFormat: false),
        new KeyValuesSnippet(parameterContents), Controller));
  }

  public OperationObjectHandler(Controller controller) : base(controller)
  {
  }
}