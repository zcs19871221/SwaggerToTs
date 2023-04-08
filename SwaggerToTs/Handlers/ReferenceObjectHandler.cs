using System.Text.RegularExpressions;
using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.Handlers;

public class ReferenceObjectHandler: Handler
{

  protected T GetRef<T>(T reference) where T : ReferenceObject
  {
    return GetRefMaybe(reference) ?? throw new InvalidOperationException();
  }

  
  protected ValueSnippet Handle<T>(T referenceObject, Func<T, ValueSnippet> create)
    where T : ReferenceObject
  {
    var reference = referenceObject.Reference;
    if (reference == null) return create(referenceObject);
    Controller.RefMappingIsolate.TryGetValue(reference, out var isolate);
    if (isolate != null)
    {
      return new ExportedValueSnippet(isolate, Controller);
    }
      
    var refObject = GetRef(referenceObject);
    var snippet = create(refObject);
    snippet.ReferenceUrl = reference;
    Controller.RefMappingIsolate.Add(reference, snippet);
    return  snippet.Export(Controller.ReferenceMappingShortName.GetValueOrDefault(reference) ?? throw new InvalidOperationException(), "data-schema", Controller);
  }
  
  public T? GetRefMaybe<T>(T reference) where T : ReferenceObject
  {
    var components = Controller.Components;

    if (components == null)
    {
      return null;
    }

    var refLink = reference.Reference ?? "";
    switch (reference)
    {
      case HeaderObject:
        return components.Headers?.GetValueOrDefault(refLink) as T;
      case ParameterObject: 
        return components.Parameters?.GetValueOrDefault(refLink) as T;
      case RequestBodyObject:
        return components.RequestBodies?.GetValueOrDefault(refLink) as T;
      case ResponseObject:
        return components.Responses?.GetValueOrDefault(refLink) as T;
      case SchemaObject:
        return components.Schemas?.GetValueOrDefault(refLink) as T;
      default:
        throw new Exception($"can't handle type:{reference.GetType()}");
    }
  }

  public ReferenceObjectHandler(Controller controller) : base(controller)
  {
  }
}