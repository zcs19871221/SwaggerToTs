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
        element is SchemaObject { SchemaType: SchemaTypeEnums.Enum } or SchemaObject { Nullable: true }
          or SchemaObject { SchemaType: SchemaTypeEnums.OneOf } or SchemaObject { SchemaType: SchemaTypeEnums.AllOf }
          or SchemaObject { SchemaType: SchemaTypeEnums.AnyOf })
      content = $"({content})";

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
    schema.SchemaType = SchemaTypeEnums.Enum;
  
    if (TsCodeWriter.Get().Options.Get<EnumUseEnum>().Value)
    {
      schema.Contents =  "{\n  " + string.Join(",\n  ", schema.Enum.Select(e =>  e + $" = '{e}'")) + "\n}";
      schema.ExportTypeValue = ExportType.Enum;
    }
    else
    {
      schema.ExportTypeValue = ExportType.Type;
      schema.Contents = string.Join(" | ", schema.Enum.Select(e =>
      {
        switch (e)
        {
          case byte:
          case sbyte:
          case ushort:
          case uint:
          case ulong:
          case short:
          case int:
          case long:
          case decimal:
          case double:
          case float:
            return $"{e}";
          default:
            return $"'{e}'";
        }
      }));
    }
 
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
    schema.SchemaType = SchemaTypeEnums.String;
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
    schema.SchemaType = schema.Type is "number" ? SchemaTypeEnums.Number : SchemaTypeEnums.Integer;

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
    schema.SchemaType = SchemaTypeEnums.Bool;
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
    schema.SchemaType = SchemaTypeEnums.Array;
    schema.ExportTypeValue = ExportType.Type;
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

public class CommonSchemaHandler
{
  protected void Add(SchemaObject schema)
  {
    schema.SchemaType = SchemaTypeEnums.Object;
    schema.AddComment(nameof(schema.MinProperties), schema.MinProperties.ToString())
      .AddComment(nameof(schema.MaxProperties), schema.MaxProperties.ToString());
  }
}

public class ObjectSchemaHandler : CommonSchemaHandler, ISchemaHandler
{
  public bool IsMatch(SchemaObject schema)
  {
    return schema.Type == "object";
  }


  public void CreateTsCode(SchemaObject schema)
  {
    Add(schema);

    schema.ExportTypeValue = ExportType.Interface;
    schema.Properties = schema.Properties.ToDictionary(a => TsCodeElement.ToCamelCase(a.Key), a => a.Value);
    schema.Merge(TsCodeElement.CreateFragment(schema.Properties, (key, o) =>
    {
      var wrapper = new TsCodeFragment
      {
        Name = key,
        ReadOnly = true,
        Optional = false,
      };
      var item = o.GenerateTsCode();
      wrapper.Optional = !schema.Required.Contains(key);
      return wrapper.Merge(item);
    }));
  }
}

  public class AnyObjectSchemaHandler : CommonSchemaHandler, ISchemaHandler
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
      schema.SchemaType = SchemaTypeEnums.Any;
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

    public void CreateTsCode(SchemaObject schema)
    {
      schema.SchemaType = SchemaTypeEnums.OneOf;
      List<TsCodeElement> contents = new();
      foreach (var schemaObject in schema.Oneof)
      {
        var each = schemaObject.GenerateTsCode();
        schema.Merge(each, element => { contents.Add(element); });
      }


      var name = TsCodeWriter.OneOfName;

      schema.Contents = name + Helper.CreateHelperGeneric(contents.Select(e => e.GenerateCodeBody()));
      schema.ImportedHelpers.Add(name);
    }
  }

  public class AnyOfHandler : ISchemaHandler
  {
    public bool IsMatch(SchemaObject schema)
    {
      return schema.AnyOf.Any();
    }


    public void CreateTsCode(SchemaObject schema)
    {
      schema.SchemaType = SchemaTypeEnums.AnyOf;
      List<TsCodeElement> contents = new();
      foreach (var schemaObject in schema.AnyOf)
      {
        var each = schemaObject.GenerateTsCode();
        schema.Merge(each, element => { contents.Add(element); });
      }

      var name = TsCodeWriter.AnyOfName;

      schema.Contents = name + Helper.CreateHelperGeneric(contents.Select(e => e.GenerateCodeBody()));
      schema.ImportedHelpers.Add(name);
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
      schema.SchemaType = SchemaTypeEnums.AllOf;
      List<TsCodeElement> contents = new();
      foreach (var schemaObject in schema.Allof)
      {
        var each = schemaObject.GenerateTsCode();
        schema.Merge(each, element => { contents.Add(element); });
      }

      schema.Contents = string.Join(" & ", contents.Select(Helper.AddBracketIfNeed));
    }
}