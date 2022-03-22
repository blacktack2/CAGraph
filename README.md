# TileGraph

## About

TileGraph is a work-in-progress graph based procedural generation tool using Unity's graph editor. This makes use of the GPU via compute shaders to run operations on 2D arrays faster.

This README will be updated with more formalized documentation at a later date when this project is less volatile in it's changes.

### Nodes

#### Input

##### TileMap Boolean

2D array of booleans (stored as integers to allow transfer to compute shaders).

##### TileMap Continuous

2D array of floats clamped between 0 and 1 (inclusive).

##### TileMap Integer

2D array of unsigned integers.

#### Operations

##### Fill

Set all elements of a given tile-map to a single specified value.

##### Replace

Replace all instances of one value in an tile-map with another.

##### Randomize

Set all elements of a given tile-map to a randomly assigned value.

##### Lifelike CA

Apply a life-like Cellular Automata (CA) (see: [Cellular Automata](https://conwaylife.com/wiki/Cellular_automaton#Life-like_cellular_automata)) with custom rules for a set number of iterations to a matrix.

##### Noise

Apply smooth noise to a tile-map. Implementation includes 3 algorithms the user can select from (Value, Perlin, and Simplex Noise).

##### Voronoi Noise

Apply voronoi noise to a tile-map.

## TODO

- Create new nodes
- Fix CPU/GPU dissimilarity for randomization algorithms
