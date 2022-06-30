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

    public static bool gravityEnabled { get; private set; }
    public static bool paused { get; private set; }

    private int LEFT_BORDER = -5;
    private int RIGHT_BORDER = 4;
    private int BOTTOM_BORDER = -10;

    private int TOP_BORDER = 9;



    public static ArrayList landedPieces = new ArrayList();

    private void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<Piece>();
        gravityEnabled = true;
        paused = false;

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
            gravityEnabled = !gravityEnabled;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
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
        clearLines();
        SpawnPiece();

        for (int i = 0; i < piece.cells.Length; i++)
        {
            landedPieces.Add(piece.cells[i] + position);
        }

    }

    public void clearLines()
    {
        for (int i = TOP_BORDER; i >= BOTTOM_BORDER; i--)
        {
            bool clearLine = true;
            for (int j = LEFT_BORDER; j <= RIGHT_BORDER; j++)
            {
                if (!tilemap.GetTile(new Vector3Int(j, i, 0)))
                {
                    clearLine = false;
                }
            }
            if (clearLine)
            {
                for (int k = LEFT_BORDER; k <= RIGHT_BORDER; k++)
                {
                    tilemap.SetTile(new Vector3Int(k, i, 0), null);
                }
                squashLines(i);
            }
        }
    }

    public void squashLines(int clearedLine)
    {
        // so every time i clear a line, i should call this method with the Y coordinate of the cleared line
        // then i traverse the tilemap going right to left going up and set each tile to the one above it
        for (int i = clearedLine; i <= TOP_BORDER; i++)
        {
            for (int j = RIGHT_BORDER; j >= LEFT_BORDER; j--)
            {
                tilemap.SetTile(new Vector3Int(j, i, 0), tilemap.GetTile(new Vector3Int(j, i + 1, 0)));
            }
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
                // print("outside left boundary");
                return false;
            }
            // inside right boundary
            else if (true_x > RIGHT_BORDER)
            {
                // print("outside right boundary");
                return false;
            }
            // inside bottom boundary
            else if (true_y < BOTTOM_BORDER)
            {
                // print("outside bottom boundary");
                return false;
            }
            else if (true_y > TOP_BORDER)
            {
                // print("outside top boundary");
                return false;
            }
            // inside landed piece

            bool tileTaken = this.tilemap.GetTile(cells[i] + position);

            if (tileTaken)
            {
                // print("contact with landed piece");
                return false;
            }

        }
        return true;
    }

}
