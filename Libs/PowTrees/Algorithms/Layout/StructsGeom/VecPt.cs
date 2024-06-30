using PowBasics.Geom;

// ReSharper disable once CheckNamespace
namespace PowTrees.Algorithms;

public readonly record struct VecPt(double X, double Y)
{
	public static readonly VecPt Zero = new(0, 0);
	public override string ToString() => $"{X},{Y}";
	public static VecPt operator +(VecPt a, VecPt b) => new(a.X + b.X, a.Y + b.Y);
	public static VecPt operator -(VecPt a, VecPt b) => new(a.X - b.X, a.Y - b.Y);
	public static VecPt operator -(VecPt a) => new(-a.X, -a.Y);
	public static VecPt operator *(VecPt a, double b) => new(a.X * b, a.Y * b);
	public static VecPt operator /(VecPt a, double b) => new(a.X / b, a.Y / b);
	public double Length => Math.Sqrt(X * X + Y * Y);
}

public static class VecPtExt
{
	public static Pt ToPt(this VecPt p) => new((int)p.X, (int)p.Y);
}