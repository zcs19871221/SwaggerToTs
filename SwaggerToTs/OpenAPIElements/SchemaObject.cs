namespace SwaggerToTs.OpenAPIElements;

public class SchemaObject:ReferenceObject
{

  public bool Nullable { get; set; }


  #region commonProperties

  public string? Type { get; set; }


  public string? Title { get; set; }
  
  public string? Description { get; set; }
  
  public bool Deprecated { get; set; }
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

public enum SchemaTypeEnums
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
  Integer,
  Any
}