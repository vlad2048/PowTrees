namespace PowTrees.LINQPad.Utils;

static class UnitExt
{
	public static string h(this int v) => $"{v}ch";
	public static string v(this int v) => $"{v}em";
	public static string h(this double v) => $"{v}ch";
	public static string v(this double v) => $"{v}em";

	internal static string hHalf(this int v) => $"{v}.5ch";
	internal static string vHalf(this int v) => $"{v}.5em";
}