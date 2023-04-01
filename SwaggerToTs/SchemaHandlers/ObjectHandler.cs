using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaHandlers;

public class ObjectHandler : ISchemaHandler
{
  public bool IsMatch(SchemaObject schema)
  {
    return schema.Type == "object";
  }


  public Snippets.WrapperSnippet Aggregate(SchemaObject schema, Controller controller)
  {
    schema.AddComment(nameof(schema.MinProperties), schema.MinProperties.ToString())
      .AddComment(nameof(schema.MaxProperties), schema.MaxProperties.ToString());
    
    return new Snippets.WrapperSnippet(schema.Properties.Select(e =>
    {
      return new Snippets.WrapperSnippet(new KeySnippet(e.Key), e.Value.Aggregate(controller));
    }));
    schema.Merge(TsCodeElement.CreateFragment(schema.Properties, (key, o) =>
    {
      new Snippets.WrapperSnippet(key, o.Aggregate())
      var wrapper = new TsCodeFragment
      {
        Name = TsCodeElement.ToCamelCase(key),
        ReadOnly = true,
        Optional = false,
      };
      var item = o.GenerateTsCode();
      wrapper.Optional = !schema.Required.Contains(key);
      return wrapper.Merge(item);
    }));
  }

  public override string ToString()
  {
    return base.ToString();
  }

  public void CreateTsCode(SchemaObject schema)
  {
    schema.SchemaType = SchemaTypeEnums.Object;
    Create(schema);
  }
}