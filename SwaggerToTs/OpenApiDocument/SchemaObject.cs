namespace SwaggerToTs.OpenApiDocument;

public class SchemaObject : TsCodeElement
{
  private static readonly List<ISchemaHandler> SchemaHandlers = new()
  {
    new EnumSchemaHandler(),
    new OneOfHandler(),
    new AnyOfHandler(),
    new AllOfHandler(),
    new StringSchemaHandler(),
    new NumberSchemaHandler(),
    new BoolSchemaHandler(),
    new ArraySchemaHandler(),
    new AnyObjectSchemaHandler(),
    new ObjectSchemaHandler(),
    new UnknownSchemaHandler()
  };

  public bool Nullable { get; set; }
  public SchemaObject(bool nullable = false)
  {
    Nullable = nullable;
    ExportTypeValue = ExportType.Type;
    ReadOnly = true;
  }
  public SchemaType? SchemaType { get; set; }


  protected override void ValidateOpenApiDocument()
  {
    if (Type == "array" && Items == null) throw new Exception("array type schema object should set Items");

    if (Type == "object")
      foreach (var requiredKey in Required)
        if (!Properties.ContainsKey(requiredKey))
          throw new Exception($"not exists required Key:{requiredKey} in property field of schemaObject");
  }

  protected override TsCodeElement CreateTsCode()
  {
    AddComment(nameof(Title), Title).AddComment(nameof(Format), Format);
    foreach (var handler in SchemaHandlers)
      if (handler.IsMatch(this))
      {
        handler.CreateTsCode(this);
        if (Nullable && ExportName == null)
        {
          Contents += " | null";
        }
        return this;
      }

    throw new Exception($"not find handler for schema type :{Type}");
  }

  #region commonProperties

  public string? Type { get; set; }


  public string? Title { get; set; }
  public List<object> Enum { get; set; } = new();
  public List<SchemaObject> Oneof { get; set; } = new();
  public List<SchemaObject> Allof { get; set; } = new();
  public List<SchemaObject> AnyOf { get; set; } = new();
  public SchemaObject? Not { get; set; }

  public string? Format { get; set; }

  #endregion

  #region stringProperties

  public string? Pattern { get; set; }
  public int? MinLength { get; set; }
  public int? MaxLength { get; set; }

  #endregion


  #region numberProperties

  public int? Minimum { get; set; }
  public int? Maximum { get; set; }
  public bool? ExclusiveMinimum { get; set; }
  public bool? ExclusiveMaximum { get; set; }
  public int? MultipleOf { get; set; }

  #endregion

  #region arrayProperties

  public SchemaObject? Items { get; set; }
  public int? MaxItems { get; set; }
  public int? MinItems { get; set; }
  public bool? UniqueItems { get; set; }

  #endregion

  #region objectProperties

  public Dictionary<string, SchemaObject> Properties { get; set; } = new();
  public List<string> Required { get; set; } = new();


  public int? MinProperties { get; set; }
  public int? MaxProperties { get; set; }

  #endregion
}

public enum SchemaType
{
  Enum,
  String,
  Number,
  Bool,
  Array,
  Object,
  OneOf,
  AnyOf,
  AllOf,
  Not,
  Any
}