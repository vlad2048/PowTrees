# PowTrees

## Table of content

- [Introduction](#introduction)
- [Usafe](#usage)
- [License](#license)



## Introduction

Tree structure with algorithms


## Usage

### Zip trees

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
