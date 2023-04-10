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
    return  snippet.Export(Controller.ReferenceMappingShortName.GetValueOrDefault(GetKey(reference)) ?? throw new InvalidOperationException(), "data-schema", Controller);
  }

  public T GetRefOrSelf<T>(T obj) where T : ReferenceObject
  {
    return GetRefMaybe(obj) ?? obj;
  }
  
  public T? GetRefMaybe<T>(T reference) where T : ReferenceObject
  {
    var components = Controller.Components;

    if (components == null)
    {
      return null;
    }

    var refLink = reference.Reference ?? "";
    if (string.IsNullOrWhiteSpace(refLink) || !refLink.Contains('/'))
    {
      return null;
    }

    var key = GetKey(refLink);

    return reference switch
    {
      HeaderObject => components.Headers?.GetValueOrDefault(key) as T,
      ParameterObject => components.Parameters?.GetValueOrDefault(key) as T,
      RequestBodyObject => components.RequestBodies?.GetValueOrDefault(key) as T,
      ResponseObject => components.Responses?.GetValueOrDefault(key) as T,
      SchemaObject => components.Schemas?.GetValueOrDefault(key) as T,
      _ => throw new Exception($"can't handle type:{reference.GetType()}")
    };
  }

  
  private string GetKey(string reference)
  {
    return reference[(reference.LastIndexOf("/", StringComparison.Ordinal) + 1)..];
  }
    public ReferenceObjectHandler(Controller controller) : base(controller)
  {
  }
}