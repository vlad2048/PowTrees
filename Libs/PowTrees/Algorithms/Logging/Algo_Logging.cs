using PowBasics.CollectionsExt;
using PowBasics.Geom;
using PowBasics.StringsExt;

// ReSharper disable once CheckNamespace
namespace PowTrees.Algorithms;

public static class Algo_Logging
{
    public static string[] Log<T>(this TNod<T> root, Action<TreeLogOpt<T>>? optFun = null)
    {
        var opt = TreeLogOpt<T>.Make(optFun);

        var layout = root.Layout(
            e => opt.FmtFun(e).GetSize(),
            layoutOpt =>
            {
                layoutOpt.GutterSz = opt.GutterSz;
                layoutOpt.AlignLevels = opt.AlignLevels;
            });
        var treeSz = layout.Values.Union().Size;
        var buffer = Enumerable.Range(0, treeSz.Height).SelectToArray(_ => new string(' ', treeSz.Width));

        void Print(R r, string s)
        {
            var sLines = s.SplitInLines();
            for (var i = 0; i < sLines.Length; i++)
            {
                var sLine = sLines[i];
                var line = buffer[r.Y + i];
                var n = sLine.Length;
                buffer[r.Y + i] = line[..r.X] + sLine + line[(r.X + n)..];
            }
        }

        foreach (var (n, r) in layout)
            Print(r, opt.FmtFun(n.V));

		ArrowUtils.DrawArrows(root, layout, (pos, str) => Print(new R(pos, new Sz(str.Length, 1)), str));

        return buffer;
    }



    private static Sz GetSize(this string str)
    {
        var lines = str.SplitInLines();
        if (lines.Length == 0) return Sz.Empty;
        return new Sz(
            lines.Max(e => e.Length),
            lines.Length
        );
    }
}
