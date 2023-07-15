# PowTrees

## Table of content

- [Introduction](#introduction)
- [Usafe](#usage)
- [License](#license)



## Introduction

Tree structure with algorithms


## Usage

### FoldL
Map a tree recursively. For each node, we use the node and the mapped parent as input

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
root.FoldL((nod, acc) => acc + nod.ParentOr(e => e.Ofs, 0), 0)
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

### Build a node lookup map
If you transform a tree (A) into tree (B) without changing its shape (just changing the node content, not children).
Very often, you then need to map the nodes of B back to A.
For this use:
```c#
static IReadOnlyDictionary<TNod<T>, TNod<U>> BuildNodeLookup<T, U>(TNod<T> rootSrc, TNod<U> rootDst);

var lookupMap = TreeUtils.BuildNodeLookup(B, A);
```


## License

MIT
