using System;
using System.Collections.Generic;
using System.Linq;

public static class IEnumerableExtensionsExtensions
{
    /// <summary>
    /// 配列・リストが空かどうか
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="self"></param>
    /// <returns></returns>
    public static bool IsEmpty<T>(this IList<T> self)
    {
        return self.Count == 0;
    }

    /// <summary>
    /// 全要素が指定した条件を満たすかどうか
    /// </summary>
    public static bool None<TSource>(
        this IEnumerable<TSource> source,
        Func<TSource, bool> predicate
    )
    {
        foreach (var n in source)
        {
            if (predicate(n))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 最小値を持つ要素をすべて返却する
    /// </summary>
    public static IEnumerable<TSource> MinElementsBy<TSource, TResult>(
        this IEnumerable<TSource> source,
        Func<TSource, TResult> selector
    )
    {
        var value = source.Min(selector);
        return source.Where(c => selector(c).Equals(value));
    }

    /// <summary>
    /// 最大値を持つ要素をすべて返却する
    /// </summary>
    public static IEnumerable<TSource> MaxElementsBy<TSource, TResult>(
        this IEnumerable<TSource> source,
        Func<TSource, TResult> selector
    )
    {
        var value = source.Max(selector);
        return source.Where(c => selector(c).Equals(value));
    }

    /// <summary>
    /// 指定されたインデックスに要素が存在する場合に true を返す
    /// </summary>
    public static bool IsDefinedAt<T>(IList<T> self, int index)
    {
        return index < self.Count;
    }

    /// <summary>
    /// 複数のシーケンスを連結して返す
    /// </summary>
    public static IEnumerable<TSource> Concat<TSource>(
        params IEnumerable<TSource>[] sources
    )
    {
        foreach (var source in sources)
        {
            foreach (var n in source)
            {
                yield return n;
            }
        }
    }

    /// <summary>
    /// 指定されたシーケンスから条件を満たさない要素を全て返す
    /// </summary>
    public static IEnumerable<TSource> WhereNot<TSource, TResult>(
        this IEnumerable<TSource> source,
        Func<TSource, bool> predicate
    )
    {
        return source.Where(c => !predicate(c));
    }

    /// <summary>
    /// シーケンスの先頭に値を追加して返す
    /// </summary>
    public static IEnumerable<TSource> StartWith<TSource>(
        this IEnumerable<TSource> source,
        params TSource[] value
    )
    {
        foreach (var n in value)
        {
            yield return n;
        }
        foreach (var n in source)
        {
            yield return n;
        }
    }

    /// <summary>
    /// シーケンスが空かどうか
    /// </summary>
    public static bool IsEmpty<TSource>(
        this IEnumerable<TSource> source
    )
    {
        return !source.Any();
    }

    /// <summary>
    /// リストの末尾から指定された数の要素を削除する
    /// </summary>
    public static void DropRight<T>(this List<T> self, int count)
    {
        self.RemoveRange(self.Count - count, count);
    }

    /// <summary>
    /// リストの先頭から指定された数の要素を削除する
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="self"></param>
    /// <param name="count"></param>
    public static void Drop<T>(this List<T> self, int count)
    {
        self.RemoveRange(0, count);
    }

    /// <summary>
    /// 最小値を持つ要素を返す
    /// </summary>
    public static TSource MinBy<TSource, TResult>(
        this IEnumerable<TSource> source,
        Func<TSource, TResult> selector
    )
    {
        var value = source.Min(selector);
        return source.First(c => selector(c).Equals(value));
    }

    /// <summary>
    /// 最大値を持つ要素を返す
    /// </summary>
    public static TSource MaxBy<TSource, TResult>(
        this IEnumerable<TSource> source,
        Func<TSource, TResult> selector
    )
    {
        var value = source.Max(selector);
        return source.First(c => selector(c).Equals(value));
    }

    /// <summary>
    /// リスト内に指定された要素があるか調べ、>存在する場合はその要素をリストから削除する
    /// </summary>
    public static void Remove<T>(
        this List<T> self,
        Predicate<T> match
    )
    {
        var index = self.FindIndex(match);
        if (index == -1)
        {
            return;
        }
        self.RemoveAt(index);
    }

    /// <summary>
    /// 配列内の要素を複数キーでソートする
    /// </summary>
    public static void Sort<TSource, TResult>(
        this TSource[] array,
        Func<TSource, TResult> selector1,
        Func<TSource, TResult> selector2) where TResult : IComparable
    {
        Array.Sort(array, (x, y) =>
        {
            var result = selector1(x).CompareTo(selector1(y));
            return result != 0 ? result : selector2(x).CompareTo(selector2(y));
        });
    }

    /// <summary>
    /// 条件を満たす場合にのみリストに要素を追加する
    /// </summary>
    public static void AddIfTrue<T>(
        this List<T> self,
        bool condition,
        T item)
    {
        if (!condition)
        {
            return;
        }
        self.Add(item);
    }

    /// <summary>
    /// List を指定されたパラメータでソートする
    /// </summary>
    public static void Sort<TSource, TResult>(
        this List<TSource> self,
        Func<TSource, TResult> selector
    ) where TResult : IComparable
    {
        self.Sort((x, y) => selector(x).CompareTo(selector(y)));
    }

    /// <summary>
    /// 指定された配列の中からランダムに要素を返す
    /// </summary>
    public static T Random<T>(params T[] values)
    {
        return values[UnityEngine.Random.Range(0, values.Length)];
    }

    /// <summary>
    /// シーケンスの中から指定した範囲の要素を抜き出す
    /// </summary>
    /// <param name="pageNumber">現在のページ番号</param>
    /// <param name="countPerPage">1 ページあたりに表示する項目の数</param>
    public static IEnumerable<T> Paging<T>(
        this IEnumerable<T> self,
        int pageNumber,
        int countPerPage)
    {
        return self
            .Skip(countPerPage * pageNumber)
            .Take(countPerPage);
    }

    /// <summary>
    /// 目的の値に最も近い値を返す
    /// </summary>
    public static int Nearest(
        this IEnumerable<int> self,
        int target
    )
    {
        var min = self.Min(c => Math.Abs(c - target));
        return self.First(c => Math.Abs(c - target) == min);
    }

    #region Nearest

    /// <summary>
    /// 目的の値に最も近い値を返す
    /// </summary>
    public static int Nearest<TSource>(
        this IEnumerable<TSource> self,
        int target,
        Func<TSource, int> selector
    )
    {
        var min = self.Min(c => Math.Abs(selector(c) - target));
        return selector(self.First(c => Math.Abs(selector(c) - target) == min));
    }

    /// <summary>
    /// 目的の値に最も近い値を持つ要素を返す
    /// </summary>
    public static TSource FindNearest<TSource>(
        this IEnumerable<TSource> self,
        int target,
        Func<TSource, int> selector
    )
    {
        var min = self.Min(c => Math.Abs(selector(c) - target));
        return self.First(c => Math.Abs(selector(c) - target) == min);
    }

    #endregion

    #region NearestMoreThan

    /// <summary>
    /// 目的の値に最も近く、目的の値より大きい値を返す
    /// </summary>
    public static int NearestMoreThan(
        this IEnumerable<int> self,
        int target
    )
    {
        var list = self.Where(c => target < c).ToArray();
        var min = list.Min(c => Math.Abs(c - target));
        return list.First(c => Math.Abs(c - target) == min);
    }

    /// <summary>
    /// 目的の値に最も近く、目的の値より大きい値を返す
    /// </summary>
    public static int NearestMoreThan<TSource>(
        this IEnumerable<TSource> self,
        int target,
        Func<TSource, int> selector
    )
    {
        var list = self.Where(c => target < selector(c)).ToArray();
        var min = list.Min(c => Math.Abs(selector(c) - target));
        return selector(list.First(c => Math.Abs(selector(c) - target) == min));
    }

    /// <summary>
    /// 目的の値に最も近く、目的の値より大きい値を持つ要素を返す
    /// </summary>
    public static TSource FindNearestMoreThan<TSource>(
        this IEnumerable<TSource> self,
        int target,
        Func<TSource, int> selector
    )
    {
        var list = self.Where(c => target < selector(c)).ToArray();
        var min = list.Min(c => Math.Abs(selector(c) - target));
        return list.First(c => Math.Abs(selector(c) - target) == min);
    }

    #endregion

    #region NearestOrMore

    /// <summary>
    /// 目的の値に最も近く、目的の値以上の値を返す
    /// </summary>
    public static int NearestOrMore(
        this IEnumerable<int> self,
        int target
    )
    {
        var list = self.Where(c => target <= c).ToArray();
        var min = list.Min(c => Math.Abs(c - target));
        return list.First(c => Math.Abs(c - target) == min);
    }

    /// <summary>
    /// 目的の値に最も近く、目的の値以上の値を返す
    /// </summary>
    public static int NearestOrMore<TSource>(
        this IEnumerable<TSource> self,
        int target,
        Func<TSource, int> selector
    )
    {
        var list = self.Where(c => target <= selector(c)).ToArray();
        var min = list.Min(c => Math.Abs(selector(c) - target));
        return selector(list.First(c => Math.Abs(selector(c) - target) == min));
    }

    /// <summary>
    /// 目的の値に最も近く、目的の値以上の値を持つ要素を返す
    /// </summary>
    public static TSource FindNearestOrMore<TSource>(
        this IEnumerable<TSource> self,
        int target,
        Func<TSource, int> selector
    )
    {
        var list = self.Where(c => target <= selector(c)).ToArray();
        var min = list.Min(c => Math.Abs(selector(c) - target));
        return list.First(c => Math.Abs(selector(c) - target) == min);
    }

    #endregion

    #region NearestOrLess

    /// <summary>
    /// 目的の値に最も近く、目的の値以下の値を返す
    /// </summary>
    public static int NearestOrLess(
        this IEnumerable<int> self,
        int target
    )
    {
        var list = self.Where(c => c <= target).ToArray();
        var min = list.Min(c => Math.Abs(c - target));
        return list.First(c => Math.Abs(c - target) == min);
    }

    /// <summary>
    /// 目的の値に最も近く、目的の値以下の値を返す
    /// </summary>
    public static int NearestOrLess<TSource>(
        this IEnumerable<TSource> self,
        int target,
        Func<TSource, int> selector
    )
    {
        var list = self.Where(c => selector(c) <= target).ToArray();
        var min = list.Min(c => Math.Abs(selector(c) - target));
        return selector(list.First(c => Math.Abs(selector(c) - target) == min));
    }

    /// <summary>
    /// 目的の値に最も近く、目的の値以下の値を持つ要素を返す
    /// </summary>
    public static TSource FindNearestOrLess<TSource>(
        this IEnumerable<TSource> self,
        int target,
        Func<TSource, int> selector
    )
    {
        var list = self.Where(c => selector(c) <= target).ToArray();
        var min = list.Min(c => Math.Abs(selector(c) - target));
        return list.First(c => Math.Abs(selector(c) - target) == min);
    }

    #endregion

    #region NearestMoreLess

    /// <summary>
    /// 目的の値に最も近く、目的の値より小さい値を返す
    /// </summary>
    public static int NearestMoreLess(
        this IEnumerable<int> self,
        int target
    )
    {
        var list = self.Where(c => c < target).ToArray();
        var min = list.Min(c => Math.Abs(c - target));
        return list.First(c => Math.Abs(c - target) == min);
    }

    /// <summary>
    /// 目的の値に最も近く、目的の値より小さい値を返す
    /// </summary>
    public static int NearestMoreLess<TSource>(
        this IEnumerable<TSource> self,
        int target,
        Func<TSource, int> selector
    )
    {
        var list = self.Where(c => selector(c) < target).ToArray();
        var min = list.Min(c => Math.Abs(selector(c) - target));
        return selector(list.First(c => Math.Abs(selector(c) - target) == min));
    }

    /// <summary>
    /// 目的の値に最も近く、目的の値より小さい値を持つ要素を返す
    /// </summary>
    public static TSource FindNearestMoreLess<TSource>(
        this IEnumerable<TSource> self,
        int target,
        Func<TSource, int> selector
    )
    {
        var list = self.Where(c => selector(c) < target).ToArray();
        var min = list.Min(c => Math.Abs(selector(c) - target));
        return list.First(c => Math.Abs(selector(c) - target) == min);
    }

    #endregion

    /// <summary>
    /// 最小値を持つ要素を返す
    /// </summary>
    public static TSource FindMin<TSource, TResult>
    (
        this IEnumerable<TSource> self,
        Func<TSource, TResult> selector
    )
    {
        return self.First(c => selector(c).Equals(self.Min(selector)));
    }

    /// <summary>
    /// 最大値を持つ要素を返す
    /// </summary>
    public static TSource FindMax<TSource, TResult>
    (
        this IEnumerable<TSource> self,
        Func<TSource, TResult> selector
    )
    {
        return self.First(c => selector(c).Equals(self.Max(selector)));
    }

    /// <summary>
    /// IList 型のインスタンスの各要素に対して、指定された処理を逆順に実行する
    /// </summary>
    public static void ForEachReverse<T>(this IList<T> self, Action<T> act)
    {
        for (int i = self.Count - 1; 0 <= i; i--)
        {
            act(self[i]);
        }
    }

    /// <summary>
    /// IList 型のインスタンスの各要素に対して、指定された処理を逆順に実行する
    /// </summary>
    public static void ForEachReverse<T>(this IList<T> self, Action<T, int> act)
    {
        for (int i = self.Count - 1; 0 <= i; i--)
        {
            act(self[i], i);
        }
    }

    /// <summary>
    /// 指定された配列内の各要素に対して、指定された処理を実行します
    /// </summary>
    /// <typeparam name="T">配列要素の型</typeparam>
    /// <param name="array">要素に処理を適用する、インデックス番号が 0 から始まる 1 次元の Array</param>
    /// <param name="action">array の各要素に対して実行する Action<T></param>
    public static void ForEach<T>(this T[] array, Action<T> action)
    {
        Array.ForEach<T>(array, obj => action(obj));
    }
}
