using System.Numerics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Piece : MonoBehaviour
{

    private float update;

    public Board board { get; private set; }
    public TetrominoData data { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int position { get; private set; }
    public bool movedDown { get; private set; }

    public bool gravityEnabled { get; private set; }

    public string keyPressed { get; private set; }

    private bool rotated;

    public void Initialize(Board board, Vector3Int position, TetrominoData data)
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

        if (this.board.paused)
        {
            return;
        }

        this.update += Time.deltaTime;
        if (this.update > 1.0f)
        {
            performTickActions();

            this.update = 0.0f;
        }

        this.board.Clear(this);

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move("down");
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move("left");
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move("right");
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Slam();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (!applySpecialRotation(this.data.tetromino))
            {
                Rotate("clockwise");
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if (!applySpecialRotation(this.data.tetromino))
            {
                Rotate("counterclockwise");
            }
        }


        this.board.Set(this);


    }

    public void performTickActions()
    {
        this.board.Clear(this);
        gravityMoveDown();
        this.board.Set(this);
    }

    public void gravityMoveDown()
    {
        if (this.board.gravityEnabled)
        {
            Move("down");
        }

    }

    public void Move(string key)
    {
        Vector2Int translation = new Vector2Int();
        this.movedDown = false;

        if (key == "down")
        {
            this.movedDown = true;
            translation = Vector2Int.down;
        }
        else if (key == "left")
        {
            translation = Vector2Int.left;
        }
        else if (key == "right")
        {
            translation = Vector2Int.right;
        }

        Vector3Int newPosition = this.position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        if (board.validateAndLand(this, newPosition))
        {
            this.position = newPosition;
        }
    }

    public void Rotate(string direction)
    {


        Vector3Int[] rotatedCells = new Vector3Int[4];
        Vector3Int rotatedPosition = this.position;
        int[] rotationData;

        if (direction == "clockwise")
        {
            rotationData = Data.ClockwiseRotationMatrix;
        }
        else
        {
            rotationData = Data.CounterClockwiseRotationMatrix;
        }

        for (int i = 0; i < cells.Length; i++)
        {
            Vector2Int rotatedVector = new Vector2Int();
            Vector3Int originalVector = cells[i];

            int rotatedX = originalVector.x * rotationData[0] + originalVector.y * rotationData[1];
            int rotatedY = originalVector.x * rotationData[2] + originalVector.y * rotationData[3];

            rotatedVector.x = rotatedX;
            rotatedVector.y = rotatedY;

            rotatedCells[i] = (Vector3Int)rotatedVector;

        }

        if (!this.board.isValidPosition(rotatedCells, rotatedPosition))
        {
            if (attemptWallKick(rotatedCells, rotatedPosition))
            {
                this.cells = rotatedCells;
            };
        }
        else
        {
            this.cells = rotatedCells;
        }

    }

    public bool applySpecialRotation(Tetromino flavor)
    {
        switch (flavor)
        {
            case Tetromino.I:
                break;
            case Tetromino.Z:
                break;
            case Tetromino.O:
                return true;
            default:
                // code block
                return false;
        }

        if (this.rotated)
        {
            for (int i = 0; i < data.cells.Length; i++)
            {
                this.cells[i] = (Vector3Int)data.cells[i];
            }
        }
        else
        {
            Vector3Int[] newCells = Data.rotatedCells[flavor];
            for (int i = 0; i < newCells.Length; i++)
            {
                this.cells[i] = newCells[i];
            }
        }

        this.rotated = !this.rotated;
        return true;

    }


    public bool attemptWallKick(Vector3Int[] rotatedCells, Vector3Int position)
    {
        Vector3Int newPosition = position;

        // try left wall kick first
        newPosition.x += Vector2Int.left.x;

        if (this.board.isValidPosition(rotatedCells, newPosition))
        {
            print("left wall kick");
            this.position = newPosition;
            return true;
        }
        // try right wall kick
        newPosition = this.position;
        newPosition.x += Vector2Int.right.x;
        if (this.board.isValidPosition(rotatedCells, newPosition))
        {
            print("right wall kick");
            this.position = newPosition;
            return true;
        }
        print("failed wall kick");
        return false;
    }

    public void Slam()
    {
        print("slam");
        Vector3Int newPosition = this.position;
        Vector2Int translation = Vector2Int.down;

        for (int i = 0; i < 20; i++)
        {
            this.movedDown = true;
            if (board.validateAndLand(this, newPosition))
            {
                ;
                this.position = newPosition;
                newPosition.x += translation.x;
                newPosition.y += translation.y;
            }
            else
            {
                break;
            }
            this.movedDown = false;

        }

    }

}
