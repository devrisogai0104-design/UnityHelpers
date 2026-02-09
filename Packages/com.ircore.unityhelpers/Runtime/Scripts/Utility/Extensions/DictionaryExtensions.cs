using System;
using System.Collections.Generic;
using System.Linq;

public static class DictionaryExtensions
{
    private static Random _random;

    private static Random random => _random ?? (_random = new Random());

    /// <summary>
    /// 引数にnullを渡してもエラーにならないContainsKeyメソッド
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="self"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static bool ContainsKeyNullable<TKey, TValue>(
        this Dictionary<TKey, TValue> self, TKey key
    )
    {
        if (key == null)
        {
            return false;
        }
        return self.ContainsKey(key);
    }

    /// <summary>
    /// ランダムなキー・値を取得する
    /// </summary>
    public static KeyValuePair<TKey, TValue> RandomAt<TKey, TValue>
    (
        this Dictionary<TKey, TValue> source
    )
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        return source.ElementAt(random.Next(0, source.Count));
    }

    /// <summary>
    /// ランダムなキーを取得する
    /// </summary>
    public static TKey RandomAt<TKey, TValue>
    (
        this Dictionary<TKey, TValue>.KeyCollection source
    )
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        return source.ElementAt(random.Next(0, source.Count));
    }

    /// <summary>
    /// ランダムな値を取得する
    /// </summary>
    public static TValue RandomAt<TKey, TValue>
    (
        this Dictionary<TKey, TValue>.ValueCollection source
    )
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        return source.ElementAt(random.Next(0, source.Count));
    }

    /// <summary>
    /// 指定したキーを持つ値を削除し、削除前に指定された関数を呼び出す
    /// </summary>
    public static void Remove<TKey, TValue>(
        this Dictionary<TKey, TValue> self,
        TKey key,
        Action<TValue> act
    )
    {
        if (!self.ContainsKey(key))
        {
            return;
        }
        act(self[key]);
        self.Remove(key);
    }

    /// <summary>
    /// 指定されたキーが格納されている場合は指定された関数を呼び出す
    /// </summary>
    public static void SafeCall<TKey, TValue>(
        this Dictionary<TKey, TValue> self,
        TKey key,
        Action<TValue> act
    )
    {
        if (!self.ContainsKey(key))
        {
            return;
        }
        act(self[key]);
    }
}
