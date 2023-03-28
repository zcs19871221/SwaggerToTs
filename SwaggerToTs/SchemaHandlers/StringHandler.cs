using SwaggerToTs.OpenAPIElements;

namespace SwaggerToTs.SchemaHandlers;

public class StringHandler : ISchemaHandler
{
  public bool IsMatch(SchemaObject schema)
  {
    return schema.Type == "string";
  }


  public void CreateTsCode(SchemaObject schema)
  {
    schema.SchemaType = SchemaTypeEnums.String;
    schema.AddComment(nameof(schema.Pattern), schema.Pattern);
    schema.AddComment(nameof(schema.MinLength), schema.MinLength.ToString());
    schema.AddComment(nameof(schema.MaxLength), schema.MaxLength.ToString());
    schema.Contents = "string";
  }
}