using System.Text.Json;
using PowBasics.Geom.Serializers;
using PowTrees.Serializer;

namespace PowTrees.Tests.Algorithms.Layout.TestSupport;

public static class TJson
{
	private static readonly JsonSerializerOptions jsonOpt = new()
	{
		WriteIndented = true,
	};
	
	static TJson()
	{
		jsonOpt.Converters.Add(new TNodSerializer<Rec>());
		jsonOpt.Converters.Add(new RSerializer());
	}
	
	public static string Ser<T>(this T obj) => JsonSerializer.Serialize(obj, jsonOpt);
	public static T Deser<T>(this string str) => JsonSerializer.Deserialize<T>(str, jsonOpt)!;
}