# CAGraph

## About

CAGraph is a work-in-progress graph based procedural generation tool using Unity's graph editor. This makes use of the GPU via compute shaders to run operations on 2D arrays faster.

### Nodes

#### Input

##### Matrix

2D array of integers. Used for input into CA nodes.

#### Operations

##### Fill

Set all elements of a given matrix to a single specified value (0 by default)

##### Randomizer

Set all elements of a given matrix to either zero or one based on a given random chance

##### Lifelike CA

Apply a life-like CA (see: [Cellular Automata](https://conwaylife.com/wiki/Cellular_automaton#Life-like_cellular_automata)) with custom rules for a set number of iterations to a matrix.

## TODO

- Create new nodes
  - Replace node
  - Set points node
- Expand CA shader to include more types of CA
  - Generation based CA
  - Continuous Automata
