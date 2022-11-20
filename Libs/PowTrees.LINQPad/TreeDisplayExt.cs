﻿/*
using LINQPad.Controls;
using PowTrees.Algorithms;
using System.Reflection;

namespace PowTrees.LINQPad;

public static class TreeDisplayExt
{
	public static Div Display<T>(this TNod<T> root) => MakeDiv(root.LogToString());

	//public static object ToDump(object o)
	//{
	//	if (o.IsGenNod()) return MakeDiv(o.LogGenNod());
	//	return o switch
	//	{
	//		R e => $"{e}",
	//		_ => o
	//	};
	//}

	private static Div MakeDiv(string text) =>
		new(new Span(text))
		{
			Styles =
			{
				["background-color"] = "#030526",
				["color"] = "#32FEC4",
				["font-family"] = "consolas",
				["font-size"] = "16px",
				["padding"] = "5px"
			}
		};
}

static class TreeDumpExt
{
	private static readonly Lazy<MethodInfo> genLogMethodDef = new(() => typeof(Algo_Logging).GetMethod("LogToString")!);
	private static MethodInfo GenLogMethodDef => genLogMethodDef.Value;

	public static bool IsGenNod(this object o)
	{
		var t = o.GetType();
		return t.IsGenericType && t.GetGenericTypeDefinition() == typeof(TNod<>);
	}

	public static string LogGenNod(this object o)
	{
		var method = GenLogMethodDef.MakeGenericMethod(o.GetGenNodType());
		var strObj = method.Invoke(null, new[] { o, null! });
		var str = strObj as string;
		return str!;
	}

	private static Type GetGenNodType(this object o)
	{
		if (!o.IsGenNod()) throw new ArgumentException();
		return o.GetType().GenericTypeArguments.Single();
	}
}
*/