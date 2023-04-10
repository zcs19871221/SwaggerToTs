using SwaggerToTs.OpenAPIElements;
using SwaggerToTs.SchemaSnippets;
using SwaggerToTs.Snippets;

namespace SwaggerToTs.SchemaHandlers;

public class NumberHandler: SchemaObjectHandler
{
  public override bool IsMatch(SchemaObject schema)
  {
    return schema.Type?.ToLower() is "number" or "integer";  
  }

  public override ValueSnippet DoConstruct(SchemaObject schema)
  {
    var snippet =  new NumberSnippet(schema);
    snippet.AddComments(new []
    {
      (nameof(schema.Maximum), schema.Maximum.ToString()),
      (nameof(schema.Maximum), schema.Maximum.ToString()),
      (nameof(schema.ExclusiveMaximum), schema.ExclusiveMaximum.ToString()),
      (nameof(schema.MultipleOf), schema.MultipleOf.ToString())
    });
    return snippet;
  }


  public NumberHandler(Controller controller) : base(controller)
  {
  }
}