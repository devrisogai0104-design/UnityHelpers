using System;

public static class IntExtensions
{
    /// <summary>
    /// 数値を指定された桁数で0埋めした文字列を返す
    /// </summary>
    /// <param name="self">対象の数値</param>
    /// <param name="numberOfDigits">桁数</param>
    /// <returns>数値を指定された桁数で0埋めした数値の文字列</returns>
    public static string ZeroFill(this int self, int numberOfDigits)
    {
        return self.ToString("D" + numberOfDigits);
    }

    /// <summary>
    /// 指定された count の回数分処理を繰り返します
    /// </summary>
    /// <param name="count">繰り返す回数</param>
    /// <param name="act">実行する Action デリゲート</param>
    public static void Times(this int count, Action act)
    {
        for (int i = 0; i < count; i++)
        {
            act();
        }
    }
}
