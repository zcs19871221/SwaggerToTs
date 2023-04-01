using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.Handlers;

public abstract class ReferenceObjectHandler: Handler
{

  protected T GetRef<T>(T reference) where T : ReferenceObject
  {
    return GetRefMaybe(reference) ?? throw new InvalidOperationException();
  }

  protected KeyValueSnippet Handle<T>(T referenceObject, Func<T, KeyValueSnippet> create) where T: ReferenceObject
  {
    var reference = referenceObject.Reference;
    if (reference == null) return create(referenceObject);
    Controller.RefMappingIsolate.TryGetValue(reference, out var isolate);
    if (isolate != null)
    {
      return new KeyValueSnippet(new ExtractedValueSnippet(isolate));
    }
      
    var refObject = GetRef(referenceObject);
    var snippet = create(refObject);
    var (isolateSnippet, valueSnippet) =  snippet.RefactorAndSave(refObject.ExportName ?? throw new InvalidOperationException(), "data-schema", Controller);
    Controller.RefMappingIsolate.Add(reference, isolateSnippet);
    return new KeyValueSnippet(valueSnippet);
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