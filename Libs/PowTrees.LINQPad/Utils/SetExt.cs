using LINQPad.Controls;
using LINQPadExtras.Utils.Exts;
using PowBasics.Geom;

namespace PowTrees.LINQPad.Utils;

static class SetExt
{
	public static C SetR<C>(this C ctrl, R r) where C : Control => ctrl
		.Set("position", "absolute")
		.Set("display", "block")
		.Set("left", $"{r.X.h()}")
		.Set("top", $"{r.Y.v()}")
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