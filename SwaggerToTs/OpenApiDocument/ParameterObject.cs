namespace SwaggerToTs.OpenApiDocument;

public class ParameterObject : TsCodeElement
{
  private string SerializeFormat = "";

  public string? In { get; set; }

  public bool? Deprecated { get; set; }
  public bool? AllowEmptyValue { get; set; }
  public string? Style { get; set; }

  public bool? Explode { get; set; }

  public bool? AllowReserved { get; set; }


  public Dictionary<string, MediaTypeObject>? Content { get; set; }


  public SchemaObject? Schema { get; set; }

  public ParameterObject(bool required = false)
  {
    Optional = !required;
    ReadOnly = true;
  }
  public static void Append(List<ParameterObject> srcParameters, List<ParameterObject> destParameters)
  {
    ValidParameterObjects(srcParameters);
    ValidParameterObjects(destParameters);

    var destKeys = destParameters.Select(e => e.GetKey()).ToList();
    foreach (var parameter in srcParameters)
      if (!destKeys.Contains(parameter.GetKey()))
        destParameters.Add(parameter);
  }

  private static void ValidParameterObjects(List<ParameterObject> parameterObjects)
  {
    if (parameterObjects.Select(e => e.GetKey()).Distinct().Count() != parameterObjects.Count)
      throw new Exception("parameterObjects has Duplicate elements");
  }

  private string GetKey()
  {
    var parameter = (ParameterObject)GetRefOrSelfIfNotExists();
    return parameter.Name + "_" + parameter.In;
  }

  public override int GetHashCode()
  {
    return $"{Name}_{In}".GetHashCode();
  }

  protected override void ValidateOpenApiDocument()
  {
    if (!string.IsNullOrWhiteSpace(Reference)) return;
    if (In == "path" && Optional == true)
      throw new Exception("need required set true when in=path in parametersObject ");

    if (In == null || Name == null) throw new Exception("name and In is required");

    if (string.IsNullOrEmpty(Name)) throw new Exception("parameter name is required!");

    ValidContent();
  }

  protected void ValidContent()
  {
    if (Content != null && Schema != null) throw new Exception("only one is required in the content and schema fields");

    if (Content?.Count > 1) throw new Exception("request content only need one item");


    if (Schema == null)
    {
      if (Content != null)
      {
        var (key, value) = Content.FirstOrDefault();
        Schema = value.Schema;
        SerializeFormat = key;
      }
    }
    else if (Style != null)
    {
      SerializeFormat = Style;
    }
  }

  protected override TsCodeElement CreateTsCode()
  {
    if (Schema == null)
    {
      return this;
    }
    AddComment(nameof(Deprecated), Deprecated.ToString())
      .AddComment(nameof(AllowEmptyValue), AllowEmptyValue.ToString())
      .AddComment(nameof(Explode), Explode.ToString())
      .AddComment(nameof(AllowReserved), AllowReserved.ToString())
      .AddComment(nameof(SerializeFormat), SerializeFormat);
    return Merge(Schema.GenerateTsCode());
  }

  public static void MergeRequestParameters(IDictionary<string, TsCodeElement> requestParameters, OperationObject operation)
  {
    Dictionary<string, List<TsCodeElement>> requestParameter = new();
    foreach (var p in operation.Parameters)
      requestParameter.GetOrCreate(((ParameterObject)p.GetRefOrSelfIfNotExists()).In ??
                                   throw new Exception("in should not be null")).Add(p);

    foreach (var (parameterType, tsCodeElements) in requestParameter)
    {
      tsCodeElements.Sort((a, b) =>
        string.CompareOrdinal(a.GetRefOrSelfIfNotExists().Name, b.GetRefOrSelfIfNotExists().Name));
      var parameter = tsCodeElements.Count == 1
        ? tsCodeElements.First().GenerateTsCode()
        : CreateFragment(tsCodeElements, (item, fragment) =>
        {
          if (item.ExportName != null)
            fragment.Extends.Add(item.ExportName);
          else
            fragment.Contents += (
              fragment.Contents.Length > 0
                ? NewLine
                : "") + item;
        });


      var type = ToPascalCase(parameterType);
      var fileLocate = operation.FileLocate;
      if (TsCodeWriter.Get().SchemaSaveToCommon)
      {
        fileLocate = $"common/{parameterType}";
      }
      parameter.ExtractTo(operation.ExportNameBase + type, fileLocate);

      requestParameters.Add(type, parameter);
    }

  }
}