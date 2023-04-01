using System.Text;
using System.Text.RegularExpressions;

namespace SwaggerToTs.Snippets;

public abstract class CommonSnippet

{

  public List<IsolateSnippet> Dependencies = new();
  
  public readonly List<(string, string)> Comments = new();
  
  public CodeLocate? CodeLocate { get; set; }
  
  public void AddComments(IEnumerable<(string, string?)> comments)
  {
    Comments.AddRange(comments.Where(e => e.Item2 != null)!);
  }

  public static string NewLine = "\n";
  public string CreateComments(List<(string, string)>? commentsToMerge = null)
  {
    var comments = Comments;
    if (commentsToMerge != null)
    {
      comments = comments.Union(commentsToMerge).ToList();
    }
    StringBuilder sb = new();
    foreach (var comment in comments)
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

  public abstract (List<IsolateSnippet>, string) Generate();
  
}

public enum CodeLocate {
  Request,
  Response,
}

