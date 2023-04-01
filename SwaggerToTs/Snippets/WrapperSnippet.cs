namespace SwaggerToTs.Snippets;

public enum SnippetType
{
    KeyValue,
    KeyWrappers,
    Wrappers
}
public class Wrapper:CommonSnippet
{

    public KeySnippet? Key { get; set; }
    public ValueSnippet? Value { get; set; }
    public IEnumerable<Wrapper>? Wrappers { get; set; }

    public SnippetType SnippetType { get; set; }
    
    private Wrapper(SnippetType type, KeySnippet? key = null, ValueSnippet? value = null, IEnumerable<Wrapper>? wrappers = null)
    {
        Key = key;
        Value = value;
        Wrappers = wrappers;
        SnippetType = type;
    }


    public static Wrapper Create(KeySnippet keySnippet, ValueSnippet valueSnippet)
    {
        var wrapper = new Wrapper(type: SnippetType.KeyValue, key: keySnippet, value: valueSnippet)
        {
            Dependencies = keySnippet.Dependencies.Union(valueSnippet.Dependencies).ToList(),
            Comments = keySnippet.Comments.Union(valueSnippet.Comments).ToList()
        };
        return wrapper;
    }

   
    public static Wrapper Create(KeySnippet keySnippet, IEnumerable<Wrapper> wrappers)
    {
        var w = wrappers.ToList();
        var newWrapper = new Wrapper(type: SnippetType.KeyWrappers, key: keySnippet, wrappers: w) {
            Dependencies = keySnippet.Dependencies.Union(AggregateDependencies(w)).ToList(),
            Comments = keySnippet.Comments
        };
        return newWrapper;
    }

    public static Wrapper Create(KeySnippet keySnippet, Wrapper wrapper)
    {
        return Create(keySnippet, new List<Wrapper>
        {
            wrapper
        });
    }
    public static Wrapper Create(IEnumerable<Wrapper> wrappers)
    {
        var w = wrappers.ToList();
        var wrapper = new Wrapper(type: SnippetType.Wrappers, wrappers:w)
        {
            Dependencies = AggregateDependencies(w),
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
    
    private static List<IsolateSnippet> AggregateDependencies(IEnumerable<Wrapper> wrappers)
    {
        return wrappers.Aggregate(new List<IsolateSnippet>(), (acc, wrapper) =>
        {
            return acc.Union(wrapper.Dependencies).ToList();
        });
    }
}

