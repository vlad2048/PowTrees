﻿using LINQPad.Controls;
using PowBasics.Geom;

namespace PowTrees.LINQPad.Utils;

static class SetExt
{
	public static C Set<C>(this C ctrl, string key, string val) where C : Control
	{
		ctrl.Styles[key] = val;
		return ctrl;
	}

	public static C SetR<C>(this C ctrl, R r) where C : Control => ctrl
		.Set("position", "absolute")
		.Set("display", "block")
		.Set("left", $"{r.X.h()}")
		.Set("top", $"{r.Y.v()}")
		.Set("width", $"{r.Width.h()}")
		.Set("height", $"{r.Height.v()}")
		.Set("line-height", $"{r.Height.v()}")
		.Set("text-align", "center");


	internal static C SetRPlusHalf<C>(this C ctrl, R r) where C : Control => ctrl
		.Set("position", "absolute")
		.Set("display", "block")
		.Set("left", $"{r.X.hHalf()}")
		.Set("top", $"{r.Y.vHalf()}")
		.Set("width", $"{r.Width.h()}")
		.Set("height", $"{r.Height.v()}")
		.Set("line-height", $"{r.Height.v()}")
		.Set("text-align", "center");

	public static Control SetWithAction(this Control ctrl, Action<Control>? optAction)
	{
		optAction?.Invoke(ctrl);
		return ctrl;
	}
}