using SwaggerToTs.Handlers;
using SwaggerToTs.SchemaSnippets;

namespace SwaggerToTs.Snippets;

public class KeyValueSnippet:ValueSnippet
{

    public KeySnippet Key { get; }
    private ValueSnippet Value { get; }

    private readonly CodeLocation _firstConstructLocation;

    public KeyValueSnippet(KeySnippet key, ValueSnippet value, Controller controller)
    {
        Key = key;
        _firstConstructLocation = controller.CurrentLocation;
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
        else if (nonNullAsRequired && !valueIsNullable && generatingInfo.InWhichIsolateSnippet.IsAppearedOnlyInResponse(_firstConstructLocation))
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

    protected override string GenerateIsolateContent(GeneratingInfo generatingInfo)
    { 
        return $"export interface {ExportName} " + AddBrackets(CreateContent(generatingInfo));
    }
}
