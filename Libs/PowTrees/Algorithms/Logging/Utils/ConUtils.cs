using System.Drawing;

// ReSharper disable once CheckNamespace
namespace PowTrees.Algorithms;

public static class ConUtils
{
	public static void PrintToConsole(this Txt txt)
	{
		foreach (var line in txt.Lines)
		{
			foreach (var chunk in line)
			{
				SetColor(chunk.Color);
				Console.Write(chunk.Text);
			}
			Console.WriteLine();
		}
	}
	
	private const char EscChar = (char)0x1B;
	private static void SetColor(Color c) => Console.Write($"{EscChar}[38;2;{c.R};{c.G};{c.B}m");
}