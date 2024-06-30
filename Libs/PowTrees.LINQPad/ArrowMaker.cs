using System.Text;
using LINQPad.Controls;
using PowBasics.Geom;
using PowTrees.Algorithms;
using PowTrees.LINQPad.DrawerLogic;
using PowTrees.LINQPad.Utils;

namespace PowTrees.LINQPad;

public static class ArrowMaker
{
	private static readonly string TreeArrowLineStroke = "#145d99";
	private static readonly string TreeArrowLineStrokeWidth = "1px";
	//private static readonly string TreeGridBorderStyle = "1px dashed #FFFFFF10";
	private static readonly string TreeArrowHeadStroke = "#145d99";
	private static readonly string TreeArrowHeadStrokeWidth = "1px";
	private static readonly string TreeArrowHeadFill = "#178ceb";
	private const string ArrowName = "arrowhead";
	private static readonly string ArrowDef = $"""
		<marker
			id="{ArrowName}"
			markerWidth ="10"
			markerHeight="10"
			refX="10"
			refY="5"
			orient="auto"
		>
			<polygon
				points = "4 3, 10 5, 4 7"
				stroke = {TreeArrowHeadStroke}
				stroke-width = {TreeArrowHeadStrokeWidth}
				fill = {TreeArrowHeadFill}
			/>
		</marker>
		""";

	
	public static Control MakeArrows<T>(this GraphLayout<T> layout)
	{
		var root = layout.Root.ToDictionary(e => e, e => e.V.R).GetRTree();
		var sb = new StringBuilder();

		void AddSvg(string str) => sb.AppendLine(str);

		void AddSvgLine(VecPt src, VecPt dst, string? markerEnd) => AddSvg($"""
		<line
			x1="{src.X.h()}"
			y1="{src.Y.v()}"
			x2="{dst.X.h()}"
			y2="{dst.Y.v()}"
			stroke={TreeArrowLineStroke}
			stroke-width={TreeArrowLineStrokeWidth}
			{(markerEnd == null ? "" : $"""marker-end="url(#{markerEnd})" """)}
		/>
	""");

		var orig = layout.BBox.Pos;

		void DrawSingleArrow(R srcR, R dstR)
		{
			srcR -= orig;
			dstR -= orig;

			var src = srcR.ToVec();
			var dst = dstR.ToVec();
			var ptSrc = src.OnTheRight();
			var ptDstAct = dst.OnTheLeft();
			var ptDst = new VecPt(ptDstAct.X, ptSrc.Y);
			AddSvgLine(ptSrc, ptDst, ArrowName);
		}

		void DrawMultipleArrows(R srcR, R[] dstRs)
		{
			srcR -= orig;
			dstRs = dstRs.SelectToArray(e => e - orig);

			var src = srcR.ToVec();
			var dsts = dstRs.Select(e => e.ToVec()).ToArray();
			var ptSrc = src.OnTheRight();
			var ptMid = new VecPt((ptSrc.X + dsts[0].Min.X) / 2, ptSrc.Y);
			var ptDsts = dsts.Select(e => e.OnTheLeft()).ToArray();
			AddSvgLine(ptSrc, ptMid, null);
			var ptConTop = new VecPt(ptMid.X, ptDsts[0].Y);
			var ptConBottom = new VecPt(ptMid.X, ptDsts[^1].Y);
			AddSvgLine(ptConTop, ptConBottom, null);
			foreach (var ptDst in ptDsts)
			{
				var ptCon = new VecPt(ptMid.X, ptDst.Y);
				AddSvgLine(ptCon, ptDst, ArrowName);
			}
		}



		root.Where(e => e.Kids.Count == 1).ForEach(e => DrawSingleArrow(e.V, e.Kids[0].V));
		root.Where(e => e.Kids.Count > 1).ForEach(e => DrawMultipleArrows(e.V, e.Kids.Select(f => f.V).ToArray()));
	

		var sz = layout.BBox.Size;

		return new Svg($"""
		<defs>
			{ArrowDef}
		</defs>
		{sb}
		""",
				0,
				0,
				$"0 0 {sz.Width}ch {sz.Height}em"
			)
			.SetRPlusHalf(layout.BBox);
	}
}