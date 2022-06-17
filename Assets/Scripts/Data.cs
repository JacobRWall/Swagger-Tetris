using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public static class Data
{
    public static readonly int[] CounterClockwiseRotationMatrix = new int[] { 0, -1, 1, 0 };
    public static readonly int[] ClockwiseRotationMatrix = new int[] { 0, 1, -1, 0 };

    public static readonly Dictionary<Tetromino, Vector2Int[]> Cells = new Dictionary<Tetromino, Vector2Int[]>()
    {
        { Tetromino.I, new Vector2Int[] { new Vector2Int(-1, 1), new Vector2Int( 0, 1), new Vector2Int( 1, 1), new Vector2Int( 2, 1) } },
        { Tetromino.J, new Vector2Int[] { new Vector2Int(-1, 1), new Vector2Int(-1, 0), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
        { Tetromino.L, new Vector2Int[] { new Vector2Int( 1, 1), new Vector2Int(-1, 0), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
        { Tetromino.O, new Vector2Int[] { new Vector2Int( 0, 1), new Vector2Int( 1, 1), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
        { Tetromino.S, new Vector2Int[] { new Vector2Int( 0, 1), new Vector2Int( 1, 1), new Vector2Int(-1, 0), new Vector2Int( 0, 0) } },
        { Tetromino.T, new Vector2Int[] { new Vector2Int( 0, 1), new Vector2Int(-1, 0), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
        { Tetromino.Z, new Vector2Int[] { new Vector2Int(-1, 1), new Vector2Int( 0, 1), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
    };


    public static readonly Dictionary<Tetromino, Vector3Int[]> rotatedCells = new Dictionary<Tetromino, Vector3Int[]>()
    {
        { Tetromino.I, new Vector3Int[] { new Vector3Int(0,1,0), new Vector3Int( 0, 2,0), new Vector3Int( 0, -1,0), new Vector3Int( 0, 0 ,0) } },
    };



}