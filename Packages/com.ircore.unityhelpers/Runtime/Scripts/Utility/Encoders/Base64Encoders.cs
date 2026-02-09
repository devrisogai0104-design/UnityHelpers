using System;
using System.Text;

/// <summary>
/// 文字列をBase64でエンコード、デコードするクラス
/// </summary>
public static class Base64Encoders
{
    private static readonly Encoding encoding = Encoding.UTF8;

    /// <summary>
    /// Base64 でエンコードする
    /// </summary>
    public static string Encode(string s)
    {
        return Convert.ToBase64String(encoding.GetBytes(s));
    }

    /// <summary>
    /// Base64 でデコードする
    /// </summary>
    public static string Decode(string s)
    {
        return encoding.GetString(Convert.FromBase64String(s));
    }
}
