namespace SwaggerToTs.Snippets;

public enum SnippetType
{
    KeyValue,
    KeyValues,
    Value,
    Values,
}
public class Snippets:CommonSnippet
{

    public KeySnippet? Key { get; set; }
    public ValueSnippet? Value { get; set; }
    public IEnumerable<Snippets>? Values { get; set; }

    public SnippetType SnippetType { get; set; }

    public Snippets(KeySnippet key, Snippets keyValue)
    {
        Key = key;
        Values = new []{keyValue};
        SnippetType = SnippetType.KeyValues;
    }
    
    public Snippets(KeySnippet key, ValueSnippet value)
    {
        Key = key;
        Value = value;
        SnippetType = SnippetType.KeyValue;
    }

    public Snippets(IEnumerable<Snippets> values)
    {
        Values = values;
        SnippetType = SnippetType.Value;
    }
    
    public Snippets(ValueSnippet valueSnippet)
    {
        Value = valueSnippet;
        SnippetType = SnippetType.Values;
    }


    public override string ToString()
    {
        switch (SnippetType)
        {
            case SnippetType.Values:
                return string.Join("\n", Values.Select(v => v.ToString()));
            case SnippetType.Value:
                return Value.ToString();
            case SnippetType.KeyValue:
                return Key.ToString() + Values.ToString();
            case SnippetType.KeyValues:
                return Key + AddBrackets(Values.ToString());
            default:
                throw new Exception(SnippetType.ToString());
        }
    }

    public (IsolateSnippet, ExtractedValueSnippet) RefactorAndSave(string exportName, string fileLocate, Controller controller)
    {
        var isolateSnippet = new IsolateSnippet(exportName, fileLocate)
        {
            Content = this
        };
        var extractedSnippet = new ExtractedValueSnippet(isolateSnippet);
        isolateSnippet.UsedBy.Add(extractedSnippet);
        controller.IsolateSnippets.Add(isolateSnippet);
        return (isolateSnippet, extractedSnippet);
    }


}

