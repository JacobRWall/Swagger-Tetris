using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class GhostBoard : Board
{

    public Ghost ghostPiece { get; private set; }
    public new Tilemap tilemap { get; private set; }

    public new TetrominoData[] tetrominos;

    public new void Set(Piece piece)
    {

        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    public new void Clear(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, null);
        }
    }

    private void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.ghostPiece = GetComponentInChildren<Ghost>();

        for (int i = 0; i < tetrominos.Length; i++)
        {
            this.tetrominos[i].Initialize();
        }
    }

    private void SpawnPiece()
    {
        this.previousPosition = this.spawnPosition;

        int random = Random.Range(0, this.tetrominos.Length);
        TetrominoData data = this.tetrominos[random];

        this.ghostPiece.Initialize(this, this.spawnPosition, data);
        Set(this.activePiece);
    }

}