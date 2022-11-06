namespace PowTrees.Algorithms.Layout.Leafifying.Structs;

record NodeMix<T>(TNod<T> Node) : IMix<T>
{
	public override string ToString() => $"n({Node})";
}