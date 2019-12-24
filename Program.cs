// fswatch Program.cs | xargs -n1 -I{} dotnet run

using System;
using System.Linq;
using System.Collections.Generic;
using static System.Sugar;

namespace Kata
{
    class Program
    {
        static void Main(string[] args)
        {
            "Hello World!".Print();
            "Done.".Print();
        }
    }
}

namespace System
{
    public class Line
    {
        public int Index { get; set; }
        public string Value { get; set; }
    }

    public class Ref<T1, T2>
    {
        public Ref() { }
        public Ref(T1 value, T2 source)
        {
            Value = value;
            Source = source;
        }
        public T1 Value { get; set; }
        public T2 Source { get; set; }
    }

    public static class Sugar
    {
        public static T[] Ary<T>(params T[] ts) => ts;

        public static T If<T>(bool predicate, T then, T els)
        {
            if (predicate) return then;
            else return els;
        }

        public static IEnumerable<int> Map(this int n)
        {
            for (int i = 0; i < n; i++) yield return i;
        }

        public static IEnumerable<T> Map<T>(this int n, Func<int, T> func) =>
            n.Map().Map(func);

        public static void Times(this int n, Action<int> action)
        {
            for (int i = 0; i < n; i++) action(n);
        }

        public static IEnumerable<T2> MapWithIndex<T1, T2>(this IEnumerable<T1> enumerable, Func<int, T1, T2> func)
        {
            var asList = enumerable.ToList();
            for (int i = 0; i < asList.Count; i++) yield return func(i, asList[i]);
        }

        public static IEnumerable<T2> Map<T1, T2>(this IEnumerable<T1> enumerable, Func<T1, T2> func)
        {
            var asList = enumerable.ToList();
            for (int i = 0; i < asList.Count; i++) yield return func(asList[i]);
        }

        public static T2 Fold<T1, T2>(this IEnumerable<T1> enumerable, T2 initialValue, Func<T2, T2> func)
        {
            var result = initialValue;
            foreach (var e in enumerable) result = func(result);
            return result;
        }

        public static T2 Fold<T1, T2>(this IEnumerable<T1> enumerable, T2 initialValue, Func<T1, T2, T2> func)
        {
            var result = initialValue;
            foreach (var e in enumerable) result = func(e, result);
            return result;
        }

        public static T SafeIndex<T>(this T[] array, int index)
        {
            if (index <  0) return default(T);
            if (index >= array.Length) return default(T);
            return array[index];
        }

        public static string Join<T>(this IEnumerable<T> enumerable, string seperator)
        {
            return string.Join(seperator, enumerable);
        }

        public static IEnumerable<T> Prints<T>(this IEnumerable<T> t)
        {
            global::System.Console.WriteLine(t.Count().ToString() + ": " + t.Join(", "));
            return t;
        }

        public static T Print<T>(this T t)
        {
            global::System.Console.WriteLine(t);
            return t;
        }

        public static IEnumerable<Line> EachLine(this string s)
        {
            return s.Split('\n').MapWithIndex((i, line) => new Line { Index = i, Value = line });
        }

        public static IEnumerable<T> EachLine<T>(this string s, Func<int, string, T> func)
        {
            return s.Split('\n').MapWithIndex(func);
        }

        public static IEnumerable<T> EachChar<T>(this string s, Func <int, string, T> func)
        {
            return s.MapWithIndex((i, c) => func(i, c.ToString()));
        }

        public static IEnumerable<T2> FlatMap<T1, T2>(this IEnumerable<T1> enumerable, Func<T1, IEnumerable<T2>> func)
        {
            var flattened = new List<T2>();
            foreach(var e in enumerable) flattened.AddRange(func(e));
            return flattened;
        }

        public static IEnumerable<Ref<T2, T1>> FlatMapRef<T1, T2>(this IEnumerable<T1> enumerable, Func<T1, IEnumerable<T2>> func)
        {
            var flattened = new List<Ref<T2, T1>>();
            foreach(var e in enumerable) flattened.AddRange(func(e).Map(t2 => new Ref<T2, T1>(value: t2, source: e)));
            return flattened;
        }

        public static int ToInt(this string n)
        {
            int result = 0;
            int.TryParse(n, out result);
            return result;
        }

        public static List<T> Remove<T>(this List<T> list, IEnumerable<T> candidates)
        {
            list.RemoveAll(l => candidates.Any(c => l.Equals(c)));
            return list;
        }

        public static List<T> Concat<T>(this IEnumerable<T> enumerable, T entry)
        {
            var list = enumerable.ToList();
            list.Add(entry);
            return list;
        }
    }
}
