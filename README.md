# PowTrees

## Table of content

- [Introduction](#introduction)
- [Usage](#usage)
- [License](#license)



## Introduction

Tree structure with algorithms


## Usage

### JSON Serialization
```c#
TNod<T> root;

var jsonOpt = new JsonSerializerOptions();
jsonOpt.Converters.Add(NodConverterFactory.Instance);

var str = JsonSerializer.Serialize(root, jsonOpt);
var rootOut = JsonSerializer.Deserialize<TNod<T>>(str, jsonOpt)!;
```

### FoldL
Map a tree recursively. For each node, we use the node and the mapped dad as input

Signature:
```c#
static TNod<U> FoldL<T, U>(
	this TNod<T> root,
	Func<TNod<T>, U, U> fun,
	U seed
);
```

Example:
```c#
record Rec(int Val, int Ofs); // where Ofs is an offset we want to apply to Val

// root =
//                 ┌►(30,0)        
//                 │               
// (10,0)──►(20,3)─┤        ┌►(50,0)
//                 └►(40,6)─┤      
//                          └►(60,1)

// 1. Apply Ofs on the node and its descendents:
// ---------------------------------------------
root.FoldL((nod, acc) => acc + nod.V.Ofs, 0)
      ┌►3    
      │      
0──►3─┤   ┌►9
      └►9─┤  
          └►10

// 2. Apply Ofs on the node descendents only:
// ------------------------------------------
root.FoldL((nod, acc) => acc + nod.DadOr(e => e.Ofs, 0), 0)
      ┌►3   
      │     
0──►0─┤   ┌►9
      └►3─┤ 
          └►9

// 3. Create a dictionary from the nodes to their accumulators
// -----------------------------------------------------------
root.Zip(root.FoldL(fun))
	.ToDictionary(
		e => e.First.V,
		e => e.Second.V
	);
```

As these cases are quite common, there are some utility functions to implement them easily:
```c#
static TNod<U> FoldL_Dad<T, U>(
	this TNod<T> root,
	Func<T, U> get,
	Func<U, U, U> fun,
	U seed
);

static IReadOnlyDictionary<T, U> FoldL_Dict<T, U>(
	this TNod<T> root,
	Func<T, U, U> fun,
	U seed
) where T : notnull;

static IReadOnlyDictionary<T, U> FoldL_Dad_Dict<T, U>(
	this TNod<T> root,
	Func<T, U> get,
	Func<U, U, U> fun,
	U seed
) where T : notnull;
```


### Build a node lookup map
If you transform a tree (A) into tree (B) without changing its shape (just changing the node content, not kidren).
Very often, you then need to map the nodes of B back to A.
For this use:
```c#
static IReadOnlyDictionary<TNod<T>, TNod<U>> BuildNodeLookup<T, U>(TNod<T> rootSrc, TNod<U> rootDst);

var lookupMap = TreeUtils.BuildNodeLookup(B, A);
```


## License

MIT
