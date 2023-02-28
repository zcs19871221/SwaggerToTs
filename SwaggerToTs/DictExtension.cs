namespace SwaggerToTs;

public static class DictExtension
{
  public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
    where TValue : new()
  {
    if (!dict.TryGetValue(key, out var val))
    {
      val = new TValue();
      dict.Add(key, val);
    }

    return val;
  }
}