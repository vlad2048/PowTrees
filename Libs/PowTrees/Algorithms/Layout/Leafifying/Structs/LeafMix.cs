namespace PowTrees.Algorithms.Layout.Leafifying.Structs;

record LeafMix<T>(TNod<T> Node, LeafMix<T>[] SubLeaves) : IMix<T>
{
	public override string ToString() => $"l({Level}-{Node})";
	public int Level => (SubLeaves.Length == 0) switch
	{
		true => 0,
		false => 1 + SubLeaves.Max(e => e.Level)
	};
}