using SwaggerToTs.OpenAPIElements;

namespace SwaggerToTs.SchemaHandlers;

public class NumberHandler : ISchemaHandler
{
  public bool IsMatch(SchemaObject schema)
  {
    return schema.Type is "number" or "integer";
  }


  public void CreateTsCode(SchemaObject schema)
  {
    schema.SchemaType = schema.Type is "number" ? SchemaTypeEnums.Number : SchemaTypeEnums.Integer;

    schema.AddComment(nameof(schema.Minimum), schema.Minimum.ToString())
      .AddComment(nameof(schema.Maximum), schema.Maximum.ToString())
      .AddComment(nameof(schema.ExclusiveMinimum), schema.ExclusiveMinimum.ToString())
      .AddComment(nameof(schema.ExclusiveMaximum), schema.ExclusiveMaximum.ToString())
      .AddComment(nameof(schema.MultipleOf), schema.MultipleOf.ToString());
    schema.Contents = "number";
  }
}