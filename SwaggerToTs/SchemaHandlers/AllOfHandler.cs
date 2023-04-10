using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.SchemaSnippets;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaHandlers;

public class AllOfHandler : SchemaObjectHandler
{
  public override bool IsMatch(SchemaObject schema)
  {
    return schema.Allof.Any();
  }

  public override ValueSnippet DoConstruct(SchemaObject schema)
  {
    var allOfs = schema.Allof.Select(Controller.SchemaObjectHandlerWrapper.Construct).ToList();
    if (Controller.SchemaObjectHandlerWrapper.ObjectHandler.IsMatch(schema))
    {
      allOfs.Add(Controller.SchemaObjectHandlerWrapper.ObjectHandler.Construct(schema));
    }

    var snippet = CreateAllOfSnippet(allOfs);
    AddCommonComments(snippet, schema);
    SetNullValue(snippet, schema);
    return snippet;
  }

  public static AllOfSnippet CreateAllOfSnippet(List<ValueSnippet> allOfs)
  {
    var exports = new List<ExportedValueSnippet>();
    foreach (var valueSnippet in allOfs)
    {
      if (valueSnippet is ExportedValueSnippet e)
      {
        exports.Add(e);
      }
    }
    var keyValues = allOfs.Where(e => e is KeyValueSnippet or ValuesSnippet).ToList();

    if (exports.Count + keyValues.Count == allOfs.Count)
    {
      return new AllOfSnippet(keyValues, exports);
    }

    return new AllOfSnippet(allOfs, null);

  }

  public AllOfHandler(Controller controller) : base(controller)
  {
  }
}