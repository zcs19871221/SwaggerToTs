using SwaggerToTs.SchemaSnippets;

namespace SwaggerToTs.Snippets;

public class KeyValueSnippet:ValueSnippet
{

    public KeySnippet Key { get; set; }
    public ValueSnippet Value { get; set; }
    
    public KeyValueSnippet(KeySnippet key, ValueSnippet value, Controller controller)
    {
        Key = key;
        Value = value;
        switch (value)
        {
            case EnumSnippet when value.ExportType == ExportType.Enum:
            case AllOfSnippet {Type: AllOfGenerateType.Interface}:
                Value = value.Export(key.Name, "data-schema", controller);
                break;
        }

        Comments = key.Comments.Concat(value.Comments).ToList();
    }

    public override string GenerateContent(Options options, List<ValueSnippet> imports)
    {
        HandleKeyRequired(options);
        var content = Value.Generate(options, imports);
        switch (Value)
        {
            case KeyValueSnippet:
            case KeyValueSnippets:
                content = AddBrackets(content);
                break;
        }
        
        return CreateComments() + Key + (Value.IsReadOnly ? "readonly " : "") +  content + (Value.IsNullable ? " | null" : "");
    }

    private void HandleKeyRequired(Options options)
    {
        var nullAsOptional = options.Get<NullAsOptional>().Value;
        var nonNullAsRequired = options.Get<NonNullAsRequired>().Value;

        if (nullAsOptional && Value.IsNullable)
        {
            Key.Required = false;
        }
        else if (CodeLocate == Snippets.CodeLocate.Response && nonNullAsRequired && !Value.IsNullable)
        {
            Key.Required = true;
        }
    }

    public override string GenerateExportedContent(Options options, List<ValueSnippet> imports)
    {
        HandleKeyRequired(options);

        return "export interface " + AddBrackets(Key + (Value.IsReadOnly ? "readonly " : "") + Value.Generate(options, imports) + (Value.IsNullable ? " | null" : ""));
    }
}
