/*
using System.Numerics;

namespace PowTrees.LINQPad.StructsGeom;

public readonly record struct Pt<T>(T X, T Y) where T : INumber<T>
{
	public override string ToString() => $"{X},{Y}";
}

public readonly record struct Sz<T>(T Width, T Height) where T : INumber<T>
{
	public override string ToString() => $"{Width},{Height}";
}

public readonly record struct R<T> where T : INumber<T>
{
	public T X { get; }
	public T Y { get; }
	public T Width { get; }
	public T Height { get; }
	public Pt<T> Pos => new(X, Y);
	public Sz<T> Size => new(Width, Height);

	public R(T x, T y, T width, T height)
	{
		X = x;
		Y = y;
		Width = width;
		Height = height;
	}

	public R(Pt<T> pos, Sz<T> size) : this(pos.X, pos.Y, size.Width, size.Height)
	{
	}

	public override string ToString() => $"{X},{Y} {Size}";
}
*/