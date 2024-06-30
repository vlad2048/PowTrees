using System.Collections;

// ReSharper disable once CheckNamespace
public static class Nod
{
	public static TNod<T> Make<T>(T v, IEnumerable<TNod<T>>? kidren = null) => new(v, kidren);
	public static TNod<T> Make<T>(T v, params TNod<T>[] kidren) => new(v, kidren);
}


public sealed class TNod<T> : IEnumerable<TNod<T>>
{
	private readonly List<TNod<T>> kids = new();

	public T V { get; private set; }
	public TNod<T>? Dad { get; private set; }
	public IReadOnlyList<TNod<T>> Kids => kids;

	internal TNod(T v, IEnumerable<TNod<T>>? kidren)
	{
		V = v;
		if (kidren != null)
			foreach (var kid in kidren)
			{
				this.kids.Add(kid);
				kid.Dad = this;
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

	public void AddKid(TNod<T> kid)
	{
		kids.Add(kid);
		kid.Dad = this;
	}

	public void InsertKid(TNod<T> kid, int index)
	{
		kids.Insert(index, kid);
		kid.Dad = this;
	}
	
	public void RemoveKid(TNod<T> kid)
	{
		kid.Dad = null;
		kids.Remove(kid);
	}

	public void ReplaceKid(TNod<T> kidPrev, TNod<T> kidNext)
	{
		var index = kids.IndexOf(kidPrev);
		if (index == -1) throw new ArgumentException();
		kidPrev.Dad = null;
		kids[index] = kidNext;
		kidNext.Dad = this;
	}

	public void ClearKidren()
	{
		foreach (var kid in Kids)
			kid.Dad = null;
		kids.Clear();
	}

	public void AddKidren(IEnumerable<TNod<T>> kids)
	{
		foreach (var kid in kids)
			AddKid(kid);
	}

	public IEnumerator<TNod<T>> GetEnumerator() => Enumerate();
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	private IEnumerator<TNod<T>> Enumerate()
	{
		IEnumerable<TNod<T>> Recurse(TNod<T> node)
		{
			yield return node;
			foreach (var kid in node.Kids)
			foreach (var kidRes in Recurse(kid))
				yield return kidRes;
		}
		foreach (var res in Recurse(this))
			yield return res;
	}
}