using SwaggerToTs.Handlers;
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
                Value = value.Export(Handler.ToPascalCase(key.Name), "data-schema", controller);
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
        var isReadOnly = Value.IsReadOnly;
        switch (Value)
        {
            case KeyValueSnippet:
            case ValuesSnippet:
            case ObjectSnippet:
                content = AddBrackets(content);
                break;
            case UnknownSnippet:
            case ExportedValueSnippet:
                isReadOnly = false;
                break;
        }

        var showNull = !options.Get<NullAsOptional>().Value && Value.IsNullable;
        return Key + (isReadOnly ? "readonly " : "") +  content + (showNull ? " | null" : "") + ";";
    }
    public override string GenerateExportedContent(Options options, GeneratingInfo generatingInfo)
    { 
        return $"export interface {ExportName} " + AddBrackets(CreateContent(options, generatingInfo));
    }
}
