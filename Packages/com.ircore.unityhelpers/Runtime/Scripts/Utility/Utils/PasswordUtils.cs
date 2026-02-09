using System;
using System.Linq;
using System.Text;

public static class PasswordUtils
{
    private const string DIGITS = "0123456789";
    private const string LOWERCASE = "abcdefghijklmnopqrstuvwxyz";
    private const string UPPERCASE = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string SYMBOLS = "!@#$%^&*()_+-=[]{}|;:,.<>?";

    private static readonly Random _random = new Random();

    /// <summary> 数値のみのパスワード（PINコード）を生成する </summary>
    public static string GenerateNumeric(int length) => Generate(length, true, false, false, false);

    /// <summary> 英数字（小文字のみ）のパスワードを生成する </summary>
    public static string GenerateAlphaNumeric(int length) => Generate(length, true, true, false, false);

    /// <summary> 英数字（大文字小文字含む）のパスワードを生成する </summary>
    public static string GenerateFullAlphaNumeric(int length) => Generate(length, true, true, true, false);

    /// <summary>
    /// 条件を指定してパスワードを生成する
    /// </summary>
    /// <param name="length">文字数</param>
    /// <param name="useDigits">数値を含めるか</param>
    /// <param name="useLower">英小文字を含めるか</param>
    /// <param name="useUpper">英大文字を含めるか</param>
    /// <param name="useSymbols">記号を含めるか</param>
    public static string Generate(
        int length,
        bool useDigits = true,
        bool useLower = true,
        bool useUpper = true,
        bool useSymbols = false)
    {
        var charPool = new StringBuilder();
        if (useDigits) charPool.Append(DIGITS);
        if (useLower) charPool.Append(LOWERCASE);
        if (useUpper) charPool.Append(UPPERCASE);
        if (useSymbols) charPool.Append(SYMBOLS);

        // 使用できる文字がない場合は空文字を返す
        if (charPool.Length == 0) return string.Empty;

        var pool = charPool.ToString();
        var result = new StringBuilder(length);

        for (int i = 0; i < length; i++)
        {
            int pos = _random.Next(pool.Length);
            result.Append(pool[pos]);
        }

        return result.ToString();
    }

    /// <summary>
    /// 読み間違えやすい文字（i, l, 1, 0, Oなど）を除外して生成する
    /// </summary>
    public static string GenerateSafePassword(int length)
    {
        const string SAFE_CHARS = "abcdefghijkmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ23456789";
        var result = new StringBuilder(length);
        for (int i = 0; i < length; i++)
        {
            result.Append(SAFE_CHARS[_random.Next(SAFE_CHARS.Length)]);
        }
        return result.ToString();
    }
}