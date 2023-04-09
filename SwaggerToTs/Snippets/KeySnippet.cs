using System.Text.RegularExpressions;

namespace SwaggerToTs.Snippets;

public class KeySnippet:CommonSnippet
{
  
  public string Name { get; set; }
  
  public bool Required { get; set; }
  
  public bool IsReadOnly { get; set; }

  public static string ToCamelCase(string name)
  {
    return char.ToLowerInvariant(name[0]) + name.Substring(1);
  }

  protected static string ToPascalCase(string name)
  {
    return char.ToUpperInvariant(name[0]) + name.Substring(1);
  }

  
  private string FormatName()
  {
    var name = ToCamelCase(Name);
    return Regex.IsMatch(Name, @"^[a-zA-Z_\d$.]+$") ? name : $"'{name}'";
  }
  public KeySnippet(string name, bool required = true, bool isReadonly = false, bool isFormat = true)
  {
    Name = name;
    Required = required;
    IsReadOnly = isReadonly;
    IsFormat = isFormat;
  }

  public bool IsFormat { get; set; }

  public override string ToString()
  {
    var outputName = IsFormat ? FormatName() : Name;
    return $"{(IsReadOnly ? "readonly " : "")}{outputName}{(Required ? "" : "?")}: ";
  }
}

