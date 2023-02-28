namespace SwaggerToTs.OpenApiDocument;

public interface ISchemaHandler
{
  public bool IsMatch(SchemaObject schema);
  public void CreateTsCode(SchemaObject schema);
}

public static class Helper
{
  public static string AddBracketIfNeed(TsCodeElement element)
  {
    var content = element.GenerateCodeBody();
    if (element.ExportName == null &&
        element is SchemaObject { SchemaType: SchemaType.Enum } or SchemaObject { Nullable: true }
          or SchemaObject { SchemaType: SchemaType.OneOf } or SchemaObject { SchemaType: SchemaType.AllOf }
          or SchemaObject { SchemaType: SchemaType.AnyOf })
    {
      content = $"({content})";
    }

    return content;
  }

  public static string CreateHelperGeneric(IEnumerable<string> values)
  {
    return $"<[{string.Join(", ", values)}]>";
  }
}

public class EnumSchemaHandler : ISchemaHandler
{
  public bool IsMatch(SchemaObject schema)
  {
    return schema.Enum.Any();
  }


  public void CreateTsCode(SchemaObject schema)
  {
    schema.SchemaType = SchemaType.Enum;
    schema.Contents = string.Join(" | ", schema.Enum.Select(e =>
    {
      switch (e)
      {
        case Byte:
        case SByte:
        case UInt16:
        case UInt32:
        case UInt64:
        case Int16:
        case Int32:
        case Int64:
        case Decimal:
        case Double:
        case Single:
          return $"{e}";
        default:
          return $"'{e}'";
      }
    }));
    schema.ExportTypeValue = ExportType.Type;
  }
}

public class StringSchemaHandler : ISchemaHandler
{
  public bool IsMatch(SchemaObject schema)
  {
    return schema.Type == "string";
  }


  public void CreateTsCode(SchemaObject schema)
  {
    schema.SchemaType = SchemaType.String;
    schema.AddComment(nameof(schema.Pattern), schema.Pattern);
    schema.AddComment(nameof(schema.MinLength), schema.MinLength.ToString());
    schema.AddComment(nameof(schema.MaxLength), schema.MaxLength.ToString());
    schema.Contents = "string";
  }
}

public class NumberSchemaHandler : ISchemaHandler
{
  public bool IsMatch(SchemaObject schema)
  {
    return schema.Type is "number" or "integer";
  }


  public void CreateTsCode(SchemaObject schema)
  {
    schema.SchemaType = SchemaType.Number;
    schema.AddComment(nameof(schema.Minimum), schema.Minimum.ToString())
      .AddComment(nameof(schema.Maximum), schema.Maximum.ToString())
      .AddComment(nameof(schema.ExclusiveMinimum), schema.ExclusiveMinimum.ToString())
      .AddComment(nameof(schema.ExclusiveMaximum), schema.ExclusiveMaximum.ToString())
      .AddComment(nameof(schema.MultipleOf), schema.MultipleOf.ToString());
    schema.Contents = "number";
  }
}

public class BoolSchemaHandler : ISchemaHandler
{
  public bool IsMatch(SchemaObject schema)
  {
    return schema.Type == "boolean";
  }


  public void CreateTsCode(SchemaObject schema)
  {
    schema.SchemaType = SchemaType.Bool;
    schema.Contents = "boolean";
  }
}

public class ArraySchemaHandler : ISchemaHandler
{
  public bool IsMatch(SchemaObject schema)
  {
    return schema.Type == "array";
  }


  public void CreateTsCode(SchemaObject schema)
  {
    schema.SchemaType = SchemaType.Array;
    schema.AddComment(nameof(schema.MaxItems), schema.MaxItems.ToString())
      .AddComment(nameof(schema.MinItems), schema.MinItems.ToString()).AddComment(nameof(schema.UniqueItems),
        schema.UniqueItems?.ToString()).ReadOnly = true;
    if (schema.Items == null) throw new Exception("array should not have empty items");

    schema.Optional ??= schema.Items.Optional;

    schema.Merge(schema.Items.GenerateTsCode(), element =>
    {
      var content = Helper.AddBracketIfNeed(element);
      schema.Contents += content + "[]";
    });
  }
}

public class CommonSchemaHandler {
  protected void Add(SchemaObject schema)
  {
    schema.SchemaType = SchemaType.Object;
    schema.AddComment(nameof(schema.MinProperties), schema.MinProperties.ToString())
      .AddComment(nameof(schema.MaxProperties), schema.MaxProperties.ToString());
  }
}
public class ObjectSchemaHandler : CommonSchemaHandler,ISchemaHandler
{
  public bool IsMatch(SchemaObject schema)
  {
    return schema.Type == "object";
  }


  public void CreateTsCode(SchemaObject schema)
  {
    Add(schema);
   
    schema.ExportTypeValue = ExportType.Interface;
    schema.Merge(TsCodeElement.CreateFragment(schema.Properties, true,
      (key, item, parent) => { parent.Optional = !schema.Required.Contains(key); }));
  }
}


public class AnyObjectSchemaHandler : CommonSchemaHandler,ISchemaHandler
{
  public bool IsMatch(SchemaObject schema)
  {
    return schema.Type == "object" && !schema.Properties.Any();
  }


  public void CreateTsCode(SchemaObject schema)
  {
    Add(schema);
    schema.Contents = @"Record<string, unknown>";
  }
}
public class UnknownSchemaHandler : ISchemaHandler
{
  public bool IsMatch(SchemaObject schema)
  {
    return true;
  }


  public void CreateTsCode(SchemaObject schema)
  {
    schema.SchemaType = SchemaType.Any;
    schema.Nullable = false;
    schema.Contents = "unknown";
  }
}

public class OneOfHandler : ISchemaHandler
{
  public bool IsMatch(SchemaObject schema)
  {
    return schema.Oneof.Any();
  }

  private static readonly Dictionary<string, TsCodeElement> Helpers = new();
  public void CreateTsCode(SchemaObject schema)
  {

    schema.SchemaType = SchemaType.OneOf;
    List<TsCodeElement> contents = new();
    foreach (var schemaObject in schema.Oneof)
    {
      var each = schemaObject.GenerateTsCode();
      schema.Merge(each, element =>
      {
        contents.Add(element);
      });
    }

   
    var name = TsCodeWriter.OneOfName;
    
    schema.Contents = name + Helper.CreateHelperGeneric(contents.Select(e => e.GenerateCodeBody()));
    schema.HelpersToImport.Add(name);
  }
}

public class AnyOfHandler : ISchemaHandler
{
  public bool IsMatch(SchemaObject schema)
  {
    return schema.AnyOf.Any();
  }
  
  
  private static readonly Dictionary<string, TsCodeElement> Helpers = new();
  public void CreateTsCode(SchemaObject schema)
  {

    schema.SchemaType = SchemaType.AnyOf;
    List<TsCodeElement> contents = new();
    foreach (var schemaObject in schema.AnyOf)
    {
      var each = schemaObject.GenerateTsCode();
      schema.Merge(each, element =>
      {
        contents.Add(element);
      });
    }

    var name = TsCodeWriter.AnyOfName;
    
    schema.Contents = name + Helper.CreateHelperGeneric(contents.Select(e => e.GenerateCodeBody()));
    schema.HelpersToImport.Add(name);
      return;
  }
}

public class AllOfHandler : ISchemaHandler
{
  public bool IsMatch(SchemaObject schema)
  {
    return schema.Allof.Any();
  }


  public void CreateTsCode(SchemaObject schema)
  {

    schema.SchemaType = SchemaType.AllOf;
    List<TsCodeElement> contents = new();
    foreach (var schemaObject in schema.Allof)
    {
      var each = schemaObject.GenerateTsCode();
      schema.Merge(each, element =>
      {
        contents.Add(element);
      });
    }

    schema.Contents = string.Join(" & ", contents.Select(Helper.AddBracketIfNeed));
  }
}