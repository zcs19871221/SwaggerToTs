using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.SchemaHandlers;
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
      operationObject.Parameters.Where(parameterObjectHandler.HasSchema).ToList();
    var parametersGroupedByType = operationObject.Parameters.GroupBy(e => parameterObjectHandler.GetRefOrSelf(e).In, (key, g) => (key, g));
    var parameterContents = parametersGroupedByType.Where(e => e.g.Any()).Select(group =>
    {
      var parameterTypeName = ToPascalCase(group.key ?? throw new InvalidOperationException());
      var groupedParametersRequired = group.g.Any(p => parameterObjectHandler.GetRefOrSelf(p).Required);
      var parameterFields = group.g.Select(p => parameterObjectHandler.Generate(p)).ToList();
      var parameterSet = parameterFields.Count == 1 ? parameterFields.First() : AllOfHandler.CreateAllOfSnippet(parameterFields);
 
      if (!Controller.Options.Get<InlineRequest>().Value)
      {
        parameterSet = parameterSet.Export(exportName + parameterTypeName, fileLocate, Controller);
      }
      return new KeyValueSnippet(new KeySnippet(parameterTypeName, required:groupedParametersRequired, isFormat:false), parameterSet, Controller) as ValueSnippet;
    }).ToList();
      

    if (operationObject.RequestBody != null && operationObject.RequestBody.Content.Count > 0)
      parameterContents.Add(requestBodyObjectHandler.Generate(operationObject.RequestBody));

    var contents = new List<KeyValueSnippet>();
    if (parameterContents.Count > 0)
    {
      contents.Add(new KeyValueSnippet(new KeySnippet("Request", isFormat:false), new ValuesSnippet(parameterContents), Controller));
      
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