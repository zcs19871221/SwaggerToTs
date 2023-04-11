using SwaggerToTs.Handlers;
using SwaggerToTs.SchemaSnippets;

namespace SwaggerToTs.Snippets;

public class KeyValueSnippet:ValueSnippet
{

    public KeySnippet Key { get; }
    private ValueSnippet Value { get; }

    
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
        CodeLocates.Add(controller.CurrentLocate);
        if (Value is ExportedValueSnippet e)
        {
            e.IsolateSnippet.CodeLocates.Add(controller.CurrentLocate);
        }
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
        var nonNullAsRequired = options.Get<NonNullResponsePropertyAsRequired>().Value;

        var valueIsNullable = Value.IsNullable;
        if (Value is ExportedValueSnippet e)
        {
            valueIsNullable = e.IsolateSnippet.IsNullable;
        }
        if (nullAsOptional && valueIsNullable)
        {
            Key.Required = false;
        }
        else if (CodeLocates.Any(codeLocate => codeLocate == CodeLocate.Response) && CodeLocates.All(codeLocate => codeLocate != CodeLocate.Request) && nonNullAsRequired && !valueIsNullable)
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

        var showNull = Value.IsNullable && !options.Get<NullAsOptional>().Value;
        return Key + (isReadOnly ? "readonly " : "") +  content + (showNull ? " | null" : "") + ";";
    }

    protected override string GenerateExportedContent(GeneratingInfo generatingInfo)
    { 
        return $"export interface {ExportName} " + AddBrackets(CreateContent(generatingInfo));
    }
}
