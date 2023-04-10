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
        Value = value switch
        {
            EnumSnippet when value.ExportType == ExportType.Enum => value.Export(Handler.ToPascalCase(key.Name),
                "data-schema", controller),
            _ => value
        };

        Comments = key.Comments.Concat(value.Comments).ToList();
    }

    protected override string GenerateContent(GeneratingInfo generatingInfo)
    {
        var content = CreateContent(generatingInfo);
        return CreateComments() + content;
    }

    private string CreateContent(GeneratingInfo generatingInfo)
    {
        var options = generatingInfo.Controller.Options;
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
        
        var content = Value.Generate(generatingInfo);
        var isReadOnly = Value.IsReadOnly;
        switch (Value)
        {
            case KeyValueSnippet:
            case KeyValuesSnippet:
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

    protected override string GenerateExportedContent(GeneratingInfo generatingInfo)
    { 
        return $"export interface {ExportName} " + AddBrackets(CreateContent(generatingInfo));
    }
}
