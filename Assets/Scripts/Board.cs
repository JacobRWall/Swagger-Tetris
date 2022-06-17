using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public Piece activePiece { get; private set; }
    public TetrominoData[] tetrominos;
    public Vector3Int spawnPosition;
    public Vector3Int previousPosition;

    public bool gravityEnabled { get; private set; }
    public bool paused { get; private set; }

    private int LEFT_BORDER = -5;
    private int RIGHT_BORDER = 4;
    private int BOTTOM_BORDER = -10;



    public ArrayList landedPieces = new ArrayList();

    private void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<Piece>();
        this.gravityEnabled = true;
        this.paused = false;

        for (int i = 0; i < tetrominos.Length; i++)
        {
            this.tetrominos[i].Initialize();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            this.tilemap.ClearAllTiles();
        }
        else if (Input.GetKeyDown(KeyCode.G))
        {
            this.gravityEnabled = !this.gravityEnabled;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            this.paused = !this.paused;
        }
    }

    private void Start()
    {
        SpawnPiece();
    }

    private void SpawnPiece()
    {
        this.previousPosition = this.spawnPosition;

        int random = Random.Range(0, this.tetrominos.Length);
        TetrominoData data = this.tetrominos[random];

        this.activePiece.Initialize(this, this.spawnPosition, data);
        Set(this.activePiece);
    }

    public void Set(Piece piece)
    {

        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    public void Clear(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, null);
        }
    }

    public void landPiece(Piece piece, Vector3Int position)
    {

        Set(piece);
        SpawnPiece();

        for (int i = 0; i < piece.cells.Length; i++)
        {
            this.landedPieces.Add(piece.cells[i] + position);
        }
    }

    public bool validateAndLand(Piece piece, Vector3Int position)
    {

        for (int i = 0; i < piece.cells.Length; i++)
        {

            int true_x = piece.cells[i].x + position.x;
            int true_y = piece.cells[i].y + position.y;

            // inside left boundary
            if (true_x < LEFT_BORDER)
            {
                print("outside left boundary");
                return false;
            }
            // inside right boundary
            else if (true_x > RIGHT_BORDER)
            {
                print("outside right boundary");
                return false;
            }
            // inside bottom boundary
            else if (true_y < BOTTOM_BORDER && piece.movedDown)
            {
                print("outside bottom boundary");
                landPiece(piece, position);
                return false;
            }
            // inside landed piece

            bool tileTaken = this.tilemap.GetTile(piece.cells[i] + position);

            if (tileTaken && piece.movedDown)
            {
                print("contact with landed piece");
                landPiece(piece, position);
                return false;
            }
            else if (tileTaken)
            {
                return false;
            }

        }
        return true;
    }


    public bool isValidPosition(Vector3Int[] cells, Vector3Int position)
    {

        for (int i = 0; i < cells.Length; i++)
        {

            int true_x = cells[i].x + position.x;
            int true_y = cells[i].y + position.y;

            // inside left boundary
            if (true_x < LEFT_BORDER)
            {
                print("outside left boundary");
                return false;
            }
            // inside right boundary
            else if (true_x > RIGHT_BORDER)
            {
                print("outside right boundary");
                return false;
            }
            // inside bottom boundary
            else if (true_y < BOTTOM_BORDER)
            {
                print("outside bottom boundary");
                return false;
            }
            // inside landed piece

            bool tileTaken = this.tilemap.GetTile(cells[i] + position);

            if (tileTaken)
            {
                print("contact with landed piece");
                return false;
            }

        }
        return true;
    }

}
