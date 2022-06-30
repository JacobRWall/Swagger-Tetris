using System.Numerics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;



public class Ghost : Piece
{

    private float update;

    public new GhostBoard board { get; private set; }

    public void Initialize(GhostBoard board, Vector3Int position, TetrominoData data)
    {
        this.board = board;
        this.position = position;
        this.data = data;
        this.rotated = false;

        if (this.cells == null)
        {
            this.cells = new Vector3Int[data.cells.Length];
        }

        for (int i = 0; i < data.cells.Length; i++)
        {
            this.cells[i] = (Vector3Int)data.cells[i];
        }
    }

    void Update()
    {

        if (Board.paused)
        {
            return;
        }

        this.board.Clear(this);

        MoveToBottom();

        this.board.Set(this);


    }


    public void setCells(Vector3Int[] cells, Vector3Int position)
    {
        this.cells = cells;
        this.position.Set(position.x, this.position.y, 0);
    }

    public void MoveToBottom()
    {
        Debug.Log("hmm");
        Vector2Int translation = new Vector2Int(0, 10);
        Vector3Int newPosition = this.position;

        while (board.isValidPosition(this.cells, newPosition))
        {
            translation = Vector2Int.down;
            newPosition.x += translation.x;
            newPosition.y += translation.y;
        }

        this.position = newPosition;
    }

}
