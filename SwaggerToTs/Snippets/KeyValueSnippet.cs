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

    public override string GenerateContent(Options options, GeneratingInfo generatingInfo)
    {
        var content = CreateContent(options, generatingInfo);
        return CreateComments() + content;
    }

    private string CreateContent(Options options, GeneratingInfo generatingInfo)
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
        
        var content = Value.Generate(options, generatingInfo);
        switch (Value)
        {
            case KeyValueSnippet:
            case KeyValueSnippets:
                content = AddBrackets(content);
                break;
            case UnknownSnippet:
                Key.IsReadOnly = false;
                break;
        }

        var showNull = !options.Get<NullAsOptional>().Value && Value.IsNullable;
        return Key + (Value.IsReadOnly ? "readonly " : "") +  content + (showNull ? " | null" : "");
    }
    public override string GenerateExportedContent(Options options, GeneratingInfo generatingInfo)
    { 
        return "export interface " + AddBrackets(CreateContent(options, generatingInfo));
    }
}
