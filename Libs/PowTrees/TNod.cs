using System.Collections;

// ReSharper disable once CheckNamespace
public static class Nod
{
	public static TNod<T> Make<T>(T v, IEnumerable<TNod<T>>? children = null) => new(v, children);
	public static TNod<T> Make<T>(T v, params TNod<T>[] children) => new(v, children);
}


public sealed class TNod<T> : IEnumerable<TNod<T>>
{
	private readonly List<TNod<T>> kids = new();

	public T V { get; private set; }
	public TNod<T>? Dad { get; private set; }
	public IReadOnlyList<TNod<T>> Kids => kids;

	internal TNod(T v, IEnumerable<TNod<T>>? children)
	{
		V = v;
		if (children != null)
			foreach (var child in children)
			{
				this.kids.Add(child);
				child.Dad = this;
			}
	}

	public override string ToString()
	{
		try
		{
			var str = $"{V}";
			return str;
		}
		catch (Exception ex)
		{
			return ex.Message;
		}
	}

	public void ChangeContent(T v) => V = v;

	public void AddChild(TNod<T> child)
	{
		kids.Add(child);
		child.Dad = this;
	}

	public void InsertChild(TNod<T> child, int index)
	{
		kids.Insert(index, child);
		child.Dad = this;
	}
	
	public void RemoveChild(TNod<T> child)
	{
		child.Dad = null;
		kids.Remove(child);
	}

	public void ReplaceChild(TNod<T> childPrev, TNod<T> childNext)
	{
		var index = kids.IndexOf(childPrev);
		if (index == -1) throw new ArgumentException();
		childPrev.Dad = null;
		kids[index] = childNext;
		childNext.Dad = this;
	}

	public void ClearChildren()
	{
		foreach (var child in Kids)
			child.Dad = null;
		kids.Clear();
	}

	public void AddChildren(IEnumerable<TNod<T>> kids)
	{
		foreach (var kid in kids)
			AddChild(kid);
	}

	public IEnumerator<TNod<T>> GetEnumerator() => Enumerate();
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	private IEnumerator<TNod<T>> Enumerate()
	{
		IEnumerable<TNod<T>> Recurse(TNod<T> node)
		{
			yield return node;
			foreach (var child in node.Kids)
			foreach (var childRes in Recurse(child))
				yield return childRes;
		}
		foreach (var res in Recurse(this))
			yield return res;
	}
}