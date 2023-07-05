using System.Drawing;
using System.Text;
using PowBasics.CollectionsExt;

// ReSharper disable once CheckNamespace
namespace PowTrees.Algorithms;


public interface ITxtWriter
{
	void Write(Txt txt);
	void Write(string text, Color color);
	void WriteLine(string text, Color color);
	void WriteLine();
	void SurroundWith(IAsciiBox box, Color color, string? title = null);
}


sealed class TxtWriter : ITxtWriter
{
	private readonly List<List<TxtChunk>> lines = new();
	private readonly List<TxtChunk> curLine = new();

	public Txt Txt
	{
		get
		{
			MakeRectangular();
			return new Txt(lines.SelectToArray(e => e.ToArray()));
		}
	}

	public void Write(Txt txt)
	{
		foreach (var line in txt.Lines)
		{
			foreach (var chunk in line)
				Write(chunk.Text, chunk.Color);
			WriteLine();
		}
	}


	public void Write(string text, Color color) => curLine.Add(new TxtChunk(text, color));

	public void WriteLine(string text, Color color)
	{
		Write(text, color);
		WriteLine();
	}

	public void WriteLine()
	{
		lines.Add(curLine.ToList());
		curLine.Clear();
	}

	public void SurroundWith(IAsciiBox box, Color color, string? title)
	{
		if (lines.Count == 0 && curLine.Count == 0) return;
		var minLng = title switch
		{
			not null => title.Length,
			null => (int?)null
		};
		var width = MakeRectangular(minLng) + 2;

		var lineTop = TxtWriterUtils.MakeTopLine(width, title, box);
		var lineBottom = TxtWriterUtils.MakeBottomLine(width, box);
		foreach (var line in lines)
		{
			line.Insert(0, new TxtChunk(box.Vert.ToString(), color));
			line.Add(new TxtChunk(box.Vert.ToString(), color));
		}
		lines.Insert(0, new List<TxtChunk> { new(lineTop, color) });
		lines.Add(new List<TxtChunk> { new(lineBottom, color) });
	}

	private int MakeRectangular(int? minLng = null)
	{
		if (lines.Count == 0 && curLine.Count == 0) return 0;
		if (curLine.Any()) WriteLine();
		int GetLng(List<TxtChunk> line) => line.Sum(e => e.Text.Length);
		var maxLng = minLng switch
		{
			null => lines.Max(GetLng),
			not null => Math.Max(minLng.Value, lines.Max(GetLng))
		};
		foreach (var line in lines)
		{
			var lineLng = GetLng(line);
			if (lineLng < maxLng)
			{
				var color = line.Any() switch
				{
					true => line.Last().Color,
					false => Color.White
				};
				line.Add(new TxtChunk(new string(' ', maxLng - lineLng), color));
			}
		}
		return maxLng;
	}
}


file static class TxtWriterUtils
{
	public static string MakeTopLine(int width, string? title, IAsciiBox box)
	{
		if (title == null) return $"{box.TL}{new string(box.Horz, width - 2)}{box.TR}";

		if (title.Length > width - 2) throw new ArgumentException();
		var (cntPrev, cntNext) = (width - 2 - title.Length).SplitInTwo();
		var sb = new StringBuilder();
		sb.Append(box.TL);
		sb.Append(new string(box.Horz, cntPrev));
		sb.Append(title);
		sb.Append(new string(box.Horz, cntNext));
		sb.Append(box.TR);
		return sb.ToString();
	}

	public static string MakeBottomLine(int width, IAsciiBox box) => $"{box.BL}{new string(box.Horz, width - 2)}{box.BR}";


	private static (int, int) SplitInTwo(this int val) => (val % 2 == 0) switch
	{
		true => (val / 2, val / 2),
		false => (val / 2, val / 2 + 1)
	};
}