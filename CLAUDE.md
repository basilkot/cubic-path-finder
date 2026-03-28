# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Rubik's Cube solver implementing BFS/DFS and A* pathfinding algorithms to find move sequences between cube states. Cube state is encoded as a 64-bit `long` (54 bits for stickers, 9 bits per face, laid out as `L_B_D_R_F_U`).

Two independent console apps under `src/`:
- **`src/bfs/`** — DFS solver with parallel task-per-move search, checkpointing, interactive CLI
- **`src/a-star/`** — A* solver with pattern database heuristics

## Build & Run

```bash
# BFS solver
dotnet build src/bfs
dotnet run --project src/bfs

# A* solver
dotnet build src/a-star
dotnet run --project src/a-star

# With CLI options (BFS)
dotnet run --project src/bfs -- f=0,1 d=8 c=3
```

.NET 8.0, C# 12. No external dependencies.

## CLI Options (BFS)

- `f=X,Y` — find specific state transitions (indices into predefined states)
- `s=N` — skip first N searches
- `d=N` — max depth N+1 (default: 7)
- `c=N` — number of solutions to find (default: 1)
- `e=M1,M2,...` — exclude moves from search
- `p[STATE]` — print moves and state outline
- `w=SOURCE M1 M2` — validate a path step-by-step

## Architecture

**State representation:** Each cube state is a `long`. Moves are `Func<long, long>` implemented via `SwapBitGroups()` bit manipulation. 18 moves total: U/R/F faces × {normal, inverse, double} + wide moves (Uu, Rr, Ff).

**SolverDeep** (BFS project): Parallelized DFS — spawns one task per initial move, each doing depth-limited recursive search. Uses `ConcurrentBag` for solutions. Supports checkpoint save/load (press S during search).

**RubikAStarSolver** (A* project): Standard A* with pattern database (PDB) heuristic for corners/edges, falling back to misplaced-stickers count.

**State matching:** Full (all 54 bits) or partial (last 27 bits = 3 faces), controlled by `SolveContext`/`SearchContext`.

**Data files in `paths/`:** `full-states.json` and `partial-states.json` store cube states as binary strings. `result.txt` accumulates found solutions. `path-to-find.txt` defines search queries.
