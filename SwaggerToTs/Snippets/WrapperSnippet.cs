namespace SwaggerToTs.Snippets;

public enum SnippetType
{
    KeyValue,
    KeyWrappers,
    Wrappers,
    Value,
}
public class WrapperSnippet:CommonSnippet
{

    public KeySnippet? Key { get; set; }
    public ValueSnippet? Value { get; set; }
    // public IEnumerable<WrapperSnippet>? Wrappers { get; set; }

    public SnippetType SnippetType { get; set; }
    
    private WrapperSnippet(SnippetType type, KeySnippet? key = null, ValueSnippet? value = null, IEnumerable<WrapperSnippet>? wrappers = null)
    {
        Key = key;
        Value = value;
        Wrappers = wrappers;
        SnippetType = type;
    }


    public static WrapperSnippet Create(KeySnippet keySnippet, ValueSnippet valueSnippet)
    {
        var wrapper = new WrapperSnippet(type: SnippetType.KeyValue, key: keySnippet, value: valueSnippet)
        {
            Dependencies = keySnippet.Dependencies.Union(valueSnippet.Dependencies).ToList(),
            Comments = keySnippet.Comments.Union(valueSnippet.Comments).ToList()
        };
        return wrapper;
    }

   
    public static WrapperSnippet Create(KeySnippet keySnippet, IEnumerable<WrapperSnippet> wrappers)
    {
        var w = wrappers.ToList();
        var newWrapper = new WrapperSnippet(type: SnippetType.KeyWrappers, key: keySnippet, wrappers: w) {
            Dependencies = keySnippet.Dependencies.Union(AggregateDependencies(w)).ToList(),
            Comments = keySnippet.Comments
        };
        return newWrapper;
    }

    public static WrapperSnippet Create(KeySnippet keySnippet, WrapperSnippet wrapperSnippet)
    {
        return Create(keySnippet, new List<WrapperSnippet>
        {
            wrapperSnippet
        });
    }
    public static WrapperSnippet Create(IEnumerable<WrapperSnippet> wrappers)
    {
        var w = wrappers.ToList();
        var wrapper = new WrapperSnippet(type: SnippetType.Wrappers, wrappers:w)
        {
            Dependencies = AggregateDependencies(w),
        };
        return wrapper;
    }

    public static WrapperSnippet Create(ValueSnippet value)
    {
        var wrapper = new WrapperSnippet(type: SnippetType.Value, value:value)
        {
            Dependencies = value.Dependencies,
            Comments = value.Comments
        };
        return wrapper;
    }
    public override string ToString()
    {
        switch (SnippetType)
        {
            case SnippetType.KeyValue:
                return CreateComments() + Key + AddBrackets(Value.ToString());
            case SnippetType.KeyWrappers:
                return CreateComments()  + Key + AddBrackets(ValuesToString());
            case SnippetType.Wrappers:
                return CreateComments() + ValuesToString();
            case SnippetType.Value:
                return Value.ToString();
            default:
                throw new Exception(SnippetType.ToString());
        }
    }

    private string ValuesToString()
    {
        return AddBrackets(string.Join(NewLine, (Wrappers ?? throw new InvalidOperationException()).Select(v => v.ToString())));
    }
   
    public ExtractedValueSnippet RefactorAndSave(string exportName, string fileLocate, Controller controller)
    {
        var isolateSnippet = new IsolateSnippet(exportName, fileLocate, this);
        var extractedSnippet = new ExtractedValueSnippet(isolateSnippet);
        isolateSnippet.UsedBy.Add(extractedSnippet);
        controller.IsolateSnippets.Add(isolateSnippet);
        return extractedSnippet;
    }
    
    private static List<IsolateSnippet> AggregateDependencies(IEnumerable<WrapperSnippet> wrappers)
    {
        return wrappers.Aggregate(new List<IsolateSnippet>(), (acc, wrapper) =>
        {
            return acc.Union(wrapper.Dependencies).ToList();
        });
    }
}

