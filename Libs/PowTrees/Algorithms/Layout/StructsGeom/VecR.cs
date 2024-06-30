using PowBasics.Geom;

// ReSharper disable once CheckNamespace
namespace PowTrees.Algorithms;

public readonly record struct VecR
{
	public static readonly VecR Empty = new(VecPt.Zero, VecPt.Zero);

	public VecPt Min { get; }
	public VecPt Max { get; }

	public double Width => Max.X - Min.X;
	public double Height => Max.Y - Min.Y;

	public VecR(VecPt min, VecPt max)
	{
		if (min.X > max.X || min.Y > max.Y)
			throw new ArgumentException();
		Min = min;
		Max = max;
	}

	public override string ToString() => $"({Min})-({Max})";

	public static VecR operator +(VecR a, VecPt b) => a == Empty ? Empty : new VecR(a.Min + b, a.Max + b);
	public static VecR operator -(VecR a, VecPt b) => a == Empty ? Empty : new VecR(a.Min - b, a.Max - b);

	public VecPt Center => new((Min.X + Max.X) / 2, (Min.Y + Max.Y) / 2);

	public bool Contains(VecPt a) => a.X >= Min.X && a.Y >= Min.Y && a.X <= Max.X && a.Y <= Max.Y;
	public VecPt Cap(VecPt a) => new(
		Cap(a.X, Min.X, Max.X),
		Cap(a.Y, Min.Y, Max.Y)
	);
	private static double Cap(double v, double min, double max) => Math.Max(Math.Min(v, max), min);
}

public static class VecRExt
{
	public static R ToR(this VecR r) => new((int)r.Min.X, (int)r.Min.Y, (int)(r.Max.X - r.Min.X), (int)(r.Max.Y - r.Min.Y));
}