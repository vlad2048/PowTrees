using PowBasics.Geom;
using PowTrees.Algorithms;
using System;
using LINQPad.Controls;
using PowTrees.LINQPad.Utils;

namespace PowTrees.LINQPad.DrawerLogic;

public static class IDrawerTreeExt
{
	public static Sz Tree<T>(
		this IDrawer draw,
		Pt pt,
		TNod<T> root,
		Func<T, Sz> szFun,
		Action<T, R> drawFun,
		Action<Algo_Layout.AlgoLayoutOpt>? optFun = null
	)
	{
		var layout = root
			.Layout(szFun, optFun)
			.Offset(pt);
		foreach (var (nod, r) in layout)
			drawFun(nod.V, r);
		draw.Arrows(layout.GetRTree());
		return root.GetSz(szFun, optFun);
	}

	public static Sz TreeCtrl<T, C>(
		this IDrawer gfx,
		Pt pt,
		TNod<T> root,
		Func<T, string> strFun,
		Func<T, string, C> makeCtrlFun,
		Action<Algo_Layout.AlgoLayoutOpt>? optFun = null
	) where C : Control
		=> gfx.Tree(
			pt,
			root,
			typ => new Sz(strFun(typ).Length, 1),
			(typ, r) => gfx.Add(
				makeCtrlFun(typ, strFun(typ))
					.SetR(gfx.UpdateDims(r))
			),
			optFun
		);
}