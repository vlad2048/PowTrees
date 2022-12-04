using System.Reflection;
using PowTrees.Algorithms;

namespace PowTrees.LINQPad;

public static class GenericTreePrinter
{
	private static readonly Lazy<MethodInfo> genLogMethodDef = new(() => typeof(Algo_Logging).GetMethod("LogToString")!);
	private static MethodInfo GenLogMethodDef => genLogMethodDef.Value;

	private static readonly Lazy<MethodInfo> genLimitDepthMethodDef = new(() => typeof(Algo_Filter).GetMethod("LimitDepth")!);
	private static MethodInfo GenLimitDepthMethodDef => genLimitDepthMethodDef.Value;

	public static bool IsTree(this object o)
	{
		var t = o.GetType();
		return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(TNod<>);
	}

	public static string Print(this object o, int? maxDepth = null)
	{
		if (maxDepth.HasValue)
		{
			var limitDepthMethod = GenLimitDepthMethodDef.MakeGenericMethod(o.GetGenNodType());
			o = limitDepthMethod.Invoke(null, new[] { o, maxDepth.Value })!;
		}

		var method = GenLogMethodDef.MakeGenericMethod(o.GetGenNodType());
		var strObj = method.Invoke(null, new[] { o, null! });
		var str = strObj as string;
		return str!;
	}

	private static Type GetGenNodType(this object o)
	{
		if (!o.IsTree()) throw new ArgumentException();
		return o.GetType().GenericTypeArguments.Single();
	}
}
