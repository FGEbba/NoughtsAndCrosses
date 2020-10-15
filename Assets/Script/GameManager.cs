using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class GameManager : MonoBehaviour
{
    [SerializeField] public char PlayerSide = 'O';
    [HideInInspector] public char CompSide = 'X';

    private int squares;
    private bool isPlayerTurn = false;
    private bool interupted = false;

    private string LastString = "";

    private int Moves = 0;
    private int MaxMoves = 0;

    private GridManager GridManager;
    private UIManager UIManager;
    private MiniMaxAI MiniMaxAI;


    private Dictionary<(int, int), GameObject> GameGrid = new Dictionary<(int, int), GameObject>();
    private Dictionary<(int, int), ButtonController> BCGrid = new Dictionary<(int, int), ButtonController>();


    void Start()
    {
        GridManager = GetComponent<GridManager>();
        UIManager = GetComponent<UIManager>();
        MiniMaxAI = GetComponent<MiniMaxAI>();

        squares = GridManager.NumberOfSquares;
        MaxMoves = squares * squares;

        UIManager.UIGridSpawn(GridManager);
        GameGrid = GridManager.GetGrid();

        MiniMaxAI.SetGameManager(this);
        UIManager.SetGameManager(this);
        SetupButtonAttachments();
        ResetBoard();
    }

    public void ResetBoard()
    {
        Moves = 0;
        LastString = "";

        SetupSides();

        MiniMaxAI.SetSides(PlayerSide, CompSide);

        foreach (KeyValuePair<(int, int), ButtonController> keyValuePair in BCGrid)
        {
            keyValuePair.Value.ResetButton();

            keyValuePair.Value.button.onClick.AddListener(delegate { ButtonOnClick(keyValuePair.Value); });
        }

        if (!isPlayerTurn)
            AIturn();
    }

    private void SetupSides()
    {
        PlayerSide = char.ToUpper(PlayerSide);

        switch (PlayerSide)
        {
            case 'O':
                CompSide = 'X';
                isPlayerTurn = false;
                break;

            case 'X':
                CompSide = 'O';
                isPlayerTurn = true;
                break;

            default:
                Debug.LogError("Not a suitable char!");
                break;
        }
    }

    void SetupButtonAttachments()
    {
        foreach (KeyValuePair<(int, int), GameObject> GridValue in GameGrid)
        {
            ButtonController bc = GridValue.Value.GetComponent<ButtonController>();

            if (bc == null)
            {
                Debug.LogError("Couldn't find \"ButtonController\" script on the button!");
                break;
            }
            bc.Setup();

            //bc.button.onClick.AddListener(action);

            BCGrid.Add(GridValue.Key, bc);

            bc.gridNumber = GridValue.Key;
            //bc.content = " ";

            //bc.button.onClick.AddListener(delegate { ButtonOnClick(bc); });
        }
    }


    void ButtonOnClick(ButtonController buttonController)
    {
        buttonController.OnClick(PlayerSide);
        Moves++;
        EndTurn();
    }

    void AIturn()
    {
        Move.Move bestMove = MiniMaxAI.FindBestMove(BCGrid, squares);
        Debug.Log($"{bestMove.col}, {bestMove.row}");
        if (bestMove.col == -1 || bestMove.row == -1)
        {
            Debug.LogError("It fuckin dieded");
            //EndTurn();
        }
        else
        {
            BCGrid[(bestMove.col, bestMove.row)].OnClick(CompSide);
            Moves++;
            EndTurn();
        }
    }

    void EndTurn()
    {
        int currentScore = MiniMaxAI.Evaluate(squares);

        if (currentScore == 10)
        {
            UIManager.OnWin();
        }

        else if (currentScore == -10)
        {
            UIManager.OnLose();
        }

        else if (Moves >= MaxMoves || !isMovesLeft())
        {
            UIManager.OnTie();
        }

        else
        {
            isPlayerTurn = !isPlayerTurn;

            if (!isPlayerTurn)
                AIturn();
        }
    }

    /* [X] [X] [X]
     * [X] [X] [ ]
     * [X] [ ] [X]
     */

    /// <summary>
    /// Check a coloumn for victories, start with a row and then it'll recursivly check the whole coloumn.
    /// </summary>
    /// <param name="coloumn">Starting Coloumn</param>
    /// <param name="row">Starting row</param>
    /// <returns>Returns 10, -10 or 0</returns>
    public int CheckColoumnsRecursivly(int coloumn, int row)
    {
        string currentChar = BCGrid[(coloumn, row)].content;

        if (coloumn >= 0 && row < squares - 1)
        {
            if (row == 0)
            {
                LastString = currentChar;
            }
            if (currentChar != " ")
            {
                if (LastString != currentChar)
                {
                    LastString = currentChar;
                    return 0;
                }
                return CheckColoumnsRecursivly(coloumn, row + 1);
            }
            return 0;
        }
        else
        {

            if (LastString == currentChar)
            {
                int value = (currentChar == PlayerSide.ToString()) ? 10 : -10;
                return value;
            }

            return 0;
        }

    }

    public int CheckRowRecursivly(int coloumn, int row)
    {
        string currentChar = BCGrid[(coloumn, row)].content;

        if (coloumn < squares - 1 && row >= 0)
        {
            if (coloumn == 0)
            {
                LastString = currentChar;
            }
            if (currentChar != " ")
            {
                if (LastString != currentChar)
                {
                    LastString = currentChar;
                    return 0;
                }
                return CheckRowRecursivly(coloumn + 1, row);
            }
            return 0;
        }
        else
        {
            if (LastString == currentChar)
            {
                int value = (currentChar == PlayerSide.ToString()) ? 10 : -10;
                return value;
            }

            interupted = false;
            return 0;
        }
    }

    public int CheckDiagnonalRecursivly(int coloumn, int row, bool StartTopRight)
    {
        string currentChar = BCGrid[(coloumn, row)].content;

        if (StartTopRight)
        {
            if (coloumn < squares - 1 && row < squares - 1)
            {
                if (coloumn == 0)
                {
                    LastString = currentChar;
                }
                if (currentChar != " ")
                {
                    if (LastString != currentChar)
                    {
                        LastString = currentChar;
                        interupted = true;
                    }
                    return CheckDiagnonalRecursivly(coloumn + 1, row + 1, true);
                }
                return 0;
            }
            else
            {
                if (LastString == currentChar && !interupted)
                {
                    int value = (currentChar == PlayerSide.ToString()) ? 10 : -10;
                    return value;
                }

                interupted = false;
                return 0;
            }
        }
        else
        {
            if (coloumn > 0 && row < squares - 1)
            {
                if (row == 0)
                {
                    LastString = currentChar;
                }
                if (currentChar != " ")
                {
                    if (LastString != currentChar)
                    {
                        LastString = currentChar;
                        interupted = true;
                    }
                    return CheckDiagnonalRecursivly(coloumn - 1, row + 1, false);
                }
                return 0;
            }
            else
            {
                if (LastString == currentChar && !interupted)
                {
                    int value = (currentChar == PlayerSide.ToString()) ? 10 : -10;
                    return value;
                }

                interupted = false;
                return 0;
            }

        }
    }
    public bool isMovesLeft()
    {
        for (int i = 0; i < 3; i++)
            for (int j = 0; j < 3; j++)
                if (BCGrid[(i, j)].content == " ")
                    return true;
        return false;
    }

}
