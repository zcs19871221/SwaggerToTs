
public interface IOption
{
  public string CommandName { get; }
  public string ShortCommandName { get; }
  public string Helper => "";
  public string Desc { get; }
  public void SetValue(string param);
  public string? DefaultValue => null;

}

public class Options
{
  public List<IOption> OptionHandlers { get; set; } = new()
  {
    new Swagger(),
    new Dist(),
    new IgnoreTags(),
    new MatchTags(),
    new PrintWidth(),
    new GuessNullable(),
    new NullableAsOptional(),
    new AggregateSchemaFile(),
  };
  
  public T Get<T>() where T: IOption
  {
    var optionHandler = OptionHandlers.Find(o => o.GetType() == typeof(T));
    if (optionHandler == null)
    {
      throw new Exception($"not add type:{typeof(T)} to optionHandlers");
    }

    return (T)optionHandler;
  }
  public Options(string[] args)
  {
    if (OptionHandlers.Select(o => o.CommandName).Distinct().Count() != OptionHandlers.Count)
    {
      throw new Exception("has duplicate commandName");
    }
    
    if (OptionHandlers.Select(o => o.ShortCommandName).Distinct().Count() != OptionHandlers.Count)
    {
      throw new Exception("has duplicate ShortCommandName");
    }
    
    List<(string,string)> arguments = new ();
    for (var i = 0; i < args.Length; i++)
    {
      var opt = args[i].Trim().Replace("-", "").Trim();
      var value = i + 1 < args.Length ? args[++i]: "";
      arguments.Add((opt, value));
    }

    var hasHandler = false;
    foreach (var (name,value) in arguments)
    {
      var optionHandler = OptionHandlers.Find(o => o.CommandName == name || o.ShortCommandName == name);
      if (optionHandler != null)
      {
        hasHandler = true;
        optionHandler.SetValue(value);
      }
    }
    if (!hasHandler)
    {
    
      var tips = string.Join("  \n", OptionHandlers.Select(e =>
        $"-{e.ShortCommandName}, --${e.CommandName}${(string.IsNullOrWhiteSpace(e.Helper) ? " " + e.Helper + " " : "").PadRight(20, ' ')} ${e.Desc}${(e.DefaultValue != null ? " default: " + e.DefaultValue : "")}").Prepend("Options:").Prepend("dotnet ts [options]"));
      Console.WriteLine(tips);
    }
    
  }
}

class Swagger : IOption
{
  public string CommandName => "swagger";
  public string ShortCommandName => "s";
  public string Desc => "swagger file locate to use";

  public string Value { get; set; } = Path.Combine(Directory.GetCurrentDirectory(), "swagger.json");

  public void SetValue(string path)
  {
    Value = Path.GetFullPath(path);
  }
  
  public bool IsRequired => true;
}
class Dist : IOption
{
  public string CommandName => "dist";
  public string ShortCommandName => "d";
  public string Desc => "generated typescript file dir";
  public void SetValue(string path)
  {
    Value = Path.GetFullPath(path);
  }
  public string Value { get; set; } = Path.Combine(Directory.GetCurrentDirectory(), "apis");
  public bool IsRequired => true;

}

class IgnoreTags : IOption
{
  public string CommandName => "ignore";
  public string ShortCommandName => "i";
  public string Desc => "tags(displayed in swagger.html) to ignore";
  public void SetValue(string args)
  {
    Value = args.Split(",").ToList();
  }

  public List<string> Value { get; set; } = new();

  public string DefaultValue => "will not skip any tags";
}

class PrintWidth : IOption
{
  public string CommandName => "printWidth";
  public string ShortCommandName => "p";
  public string Desc => "print width(same with configuration in prettier) to format";
  public void SetValue(string args)
  {
    Value = int.Parse(args);
  }

  private const int Default = 80;
  public int Value { get; set; } = Default;

  public string DefaultValue => Default.ToString();
}


class MatchTags : IOption
{
  public string CommandName => "match";
  public string ShortCommandName => "m";
  public string Desc => "tags(displayed in swagger.html) to match";
  public void SetValue(string args)
  {
    Value = args.Split(",").ToList();
  }

  public List<string> Value { get; set; } = new();
  
  public string DefaultValue => "will include all tags";
}

class BoolHandler
{
  public void SetValue(string args)
  {
    Value = true;
  }

  public bool Value { get; set; }
  public string DefaultValue => "false";
}
class GuessNullable : BoolHandler,IOption
{
  public string CommandName => "guessRequire";
  public string ShortCommandName => "g";
  public string Desc => "guess if one property is nullable (only needed in MDM since mdm doesn't set Nullable:enable in .csproj ";
}
class NullableAsOptional : BoolHandler,IOption
{
  public string CommandName => "nullableAsOptional";
  public string ShortCommandName => "n";
  public string Desc => "make nullable reference optional(if set NullValueHandling.Ignore in newtonsoft looks like only mdm set this configuration)";
}
class AggregateSchemaFile : BoolHandler, IOption
{
  public string CommandName => "aggregate";
  public string ShortCommandName => "a";
  public string Desc => "save reference file to common files, default is save to the files group by swagger tags";

}




