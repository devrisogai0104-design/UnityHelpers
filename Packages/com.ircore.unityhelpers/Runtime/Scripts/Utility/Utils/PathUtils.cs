using System.IO;

public static class PathUtils
{
    /// <summary>
    /// 指定されたパス文字列から拡張子を削除して返す
    /// </summary>
    public static string GetPathWithoutExtension(string path)
    {
        var extension = Path.GetExtension(path);
        if (string.IsNullOrEmpty(extension))
        {
            return path;
        }
        return path.Replace(extension, string.Empty);
    }
}
