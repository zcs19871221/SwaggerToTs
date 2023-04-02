using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaSnippets;


public class OneOfSnippet:CommonSnippet, IValueSnippet
{

  public IEnumerable<IValueSnippet> OneOfs;

  public OneOfSnippet(SchemaObject schema)
  {
    OneOfs = schema.Oneof.Select(e => SchemaHandlers.create(e));
    IsNullable = 
  }

  public override string ToString(Options options, List<IsolateSnippet> imports)
  {
    var contens = new List<string>();
    imports.AddRange(Dependencies);
    imports.Add(new IsolateSnippet(){"oneOf"})
    foreach (var valueSnippet in OneOfs)
    {
      contens.Add(valueSnippet.ToString());
      imports.Add(valueSnippet.Dependencies);
    }

    if (OneOfs.Any(e => e.IsNullable))
    {
      IsNullable = true;
    }

    return $"OneOf<{string.Join(",", contens)}>";
  }


  public bool IsNullable { get; set; }
  public bool IsReadOnly { get; set; }
}

public class Import
{
  
}

