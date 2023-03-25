
public interface IOption
{
  public string CommandName { get; }
  public string ShortCommandName { get; }
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
    new NullableAsOptional(),
    new EnumUseEnum(),
    new Helper()
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
    
    Stack<(string,string)> arguments = new ();
    foreach (var arg in args)
    {
      if (arg.Contains('-'))
      {
        arguments.Push((arg.Trim().Replace("-", "").Trim(), ""));
      }
      else
      {
        var last = arguments.Pop();
        last.Item2 = arg;
        arguments.Push(last);
      }
    }

    var tips = string.Join("\n  ", OptionHandlers.Select(e =>
      $"-{e.ShortCommandName,-5}, --{e.CommandName}".PadRight(35, ' ') +  $"{e.Desc}{(e.DefaultValue != null ? " (default: " + e.DefaultValue + ")" : "")}").Prepend("Options:").Prepend("dotnet ts [options]"));
    foreach (var (name,value) in arguments)
    {
      var optionHandler = OptionHandlers.Find(o => o.CommandName == name || o.ShortCommandName == name);
      if (optionHandler == null)
      {
        throw new Exception($"doesn't have options: - {name} \n\n ${tips} \n\n");
      }
      optionHandler.SetValue(optionHandler is Helper ? tips : value);
    }
  }
}

class Swagger : IOption
{
  public string CommandName => "swagger";
  public string ShortCommandName => "s";
  public string Desc => "swagger file locate to use";

  private static readonly string DefaultPath = Path.Combine(Directory.GetCurrentDirectory(), "swagger.json");
  public string Value { get; set; } = DefaultPath;

  public string DefaultValue => DefaultPath;
  

  public void SetValue(string path)
  {
    Value = Path.GetFullPath(path);
  }
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

  private static readonly string DefaultPath = Path.Combine(Directory.GetCurrentDirectory(), "apis");
  public string Value { get; set; } = DefaultPath;

  public string DefaultValue => DefaultPath;
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
}
class NullableAsOptional : BoolHandler,IOption
{
  public string CommandName => "nullableAsOptional";
  public string ShortCommandName => "n";
  public string Desc => "make nullable reference optional(both in request and response) - set this when the NullValueHandling.Ignore configuration in newtonsoft is added (default: false)";
}

class EnumUseEnum : BoolHandler, IOption
{
  public string CommandName => "enumUseEnum";
  public string ShortCommandName => "eUe";
  public string Desc => "Convert enumerated types to Enum type (default: Convert enumerated types to union types)";

}

class Helper : IOption
{
  public string CommandName => "help";
  public string ShortCommandName => "h";
  public string Desc => "show all options";

  public string Value { get; set; } = "";
  public void SetValue(string param)
  {
    Value = param;
  }
}





