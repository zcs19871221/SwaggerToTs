using System.Text.RegularExpressions;

namespace SwaggerToTs.Snippets;

public class KeySnippet:CommonSnippet
{
  
  public string Name { get; }
  
  public bool Required { get; set; }

  private bool IsReadOnly { get; }

  private static string ToCamelCase(string name)
  {
    return char.ToLowerInvariant(name[0]) + name.Substring(1);
  }


  private string FormatName(string name)
  {
    name = ToCamelCase(name);
    return Regex.IsMatch(name, @"^[a-zA-Z_\d$.]+$") ? name : $"'{name}'";
  }
  public KeySnippet(string name, bool required = true, bool isReadonly = false, bool isFormat = true)
  {
    Name = isFormat ? FormatName(name) : name;
    Required = required;
    IsReadOnly = isReadonly;
  }


  public override string ToString()
  {
    return $"{(IsReadOnly ? "readonly " : "")}{Name}{(Required ? "" : "?")}: ";
  }
}

