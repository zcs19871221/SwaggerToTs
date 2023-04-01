using System.Text;
using System.Text.RegularExpressions;

namespace SwaggerToTs.Snippets;

public class CommonSnippet

{

  public List<IsolateSnippet> Dependencies = new();
  
  public List<(string, string)> Comments = new();
  
  public CodeLocate? CodeLocate { get; set; }
  
  public void AddComments(IEnumerable<(string, string?)> comments)
  {
    Comments.AddRange(comments.Where(e => e.Item2 != null)!);
  }

  public static string NewLine = "\n";
  public string CreateComments()
  {
    StringBuilder sb = new();
    foreach (var comment in Comments)
    {
      var commentBody = comment.Item2.Replace("*/", "*\\/");
      var lines = Regex.Split(commentBody, "\n|\r\n");
      sb.AppendLine($"{(sb.Length == 0 ? "/**" : " *")}");
      sb.AppendLine($" * @{comment.Item1} {lines.First()}");
      for (var i = 1; i < lines.Length; i++) sb.AppendLine($" * {lines[i]}");
    }

    if (sb.Length > 0) sb.Append(" */").Append(NewLine);
    
    return sb.ToString();
  }
  
  public string AddBrackets(string content)
  {
    if (string.IsNullOrWhiteSpace(content) || content.StartsWith("{")) return content;
    var contents = content.Split(NewLine).ToList();
    for (var i = 0; i < contents.Count; i++) contents[i] = $"  {contents[i]}";

    contents.Insert(0, "{");
    contents.Add("}");
    return string.Join(NewLine, contents);
  }
  
}

public enum CodeLocate {
  Request,
  Response,
}

