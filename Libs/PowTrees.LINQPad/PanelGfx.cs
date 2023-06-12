using LINQPad.Controls;
using PowTrees.LINQPad.DrawerLogic;
using PowTrees.LINQPad.Utils;

namespace PowTrees.LINQPad;

public static class PanelGfx
{
	public static Control Make(Action<IDrawer> action)
	{
		var drawer = new Drawer();
		action(drawer);
		return new Div(drawer.Ctrls)
			.Set("position", "relative")
			.Set("width", $"{drawer.Size.Width.h()}")
			.Set("height", $"{drawer.Size.Height.v()}");
	}
}