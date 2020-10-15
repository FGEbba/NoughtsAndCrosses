using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Move
{
    public class Move
    {
        public int row, col;
    }
}

public class MiniMaxAI : MonoBehaviour
{
    private char player, comp;

    GameManager GameManager;
    public void SetGameManager(GameManager value) { GameManager = value; }

    public void SetSides(char playerSide, char compSide) { player = playerSide; comp = compSide; }



    /*
     * function Minimax(node, depth, maximizingPlayer) is
     *  if depth = 0 or node is a terminal node then
     *      return the heuristic value of node
     *  if maximizingPlayer then
     *      value := -infinity
     *      for each child of node do
     *          value := max(value, minimax(child, depth - 1, false))
     *      return value
     *  else (minimizing player)
     *      value := + infinity
     *      for each child of node do
     *          value := min(value, minimax(child, depth - 1, true))
     *      return value
     * 
     * 
     * inital call: minimax(origin, depth, true)
     */
    public int Evaluate(int squares)
    {
        int score;
        //Check Rows for victories
        for (int row = 0; row < squares; row++)
        {
            score = GameManager.CheckRowRecursivly(0, row);
            if (score != 0)
            {
                //Debug.Log("Row WIn");
                return score;
            }
        }

        //Check Coloumns for victories
        for (int col = 0; col < squares; col++)
        {
            score = GameManager.CheckColoumnsRecursivly(col, 0);
            if (score != 0)
            {
                //Debug.Log("Col WIn");
                return score;

            }
        }

        //Check Diagonals for victories
        score = GameManager.CheckDiagnonalRecursivly(0, 0, true);
        if (score != 0)
        {
            //Debug.Log("Diagnoal 1 WIn");
            return score;
        }

        score = GameManager.CheckDiagnonalRecursivly(squares - 1, 0, false);
        if (score != 0)
        {
            //Debug.Log("Diagnoal 2 WIn");
            return score;
        }

        return 0;
    }

    private int MiniMax(Dictionary<(int, int), ButtonController> grid, float depth, bool isMax, int squares)
    {
        int score = Evaluate(squares);

        if (score == 10)
            return score;

        if (score == -10)
            return score;

        if (!GameManager.isMovesLeft())
            return 0;

        if (isMax)
        {
            int best = -1000;

            for (int i = 0; i < squares; i++)
            {
                for (int j = 0; j < squares; j++)
                {
                    if (grid[(i, j)].content == " ")
                    {
                        //Make the move
                        grid[(i, j)].content = player.ToString();

                        best = Mathf.Max(best, MiniMax(grid, depth + 1, !isMax, squares));

                        //Undo the move
                        grid[(i, j)].content = " ";

                    }

                }
            }

            return best;

        }

        //If it's the minimizer's move
        else
        {
            int best = 1000;
            for (int i = 0; i < squares; i++)
            {
                for (int j = 0; j < squares; j++)
                {
                    if (grid[(i, j)].content == " ")
                    {
                        //Make the move
                        grid[(i, j)].content = comp.ToString();

                        best = Mathf.Min(best, MiniMax(grid, depth + 1, !isMax, squares));

                        //Undo the move
                        grid[(i, j)].content = " ";

                    }

                }
            }

            return best;
        }

    }

    public Move.Move FindBestMove(Dictionary<(int, int), ButtonController> grid, int squares)
    {
        int bestVal = -1000;
        Move.Move bestMove = new Move.Move();
        bestMove.row = -1;
        bestMove.col = -1;

        /* 
         * Traverse all cells, evaluate minimax function for all empty cells. 
         * Return teh cell with optimal value.
         */
        for (int i = 0; i < squares; i++)
        {
            for (int j = 0; j < squares; j++)
            {
                if (grid[(i, j)].content == " ")
                {
                    //Make the move
                    grid[(i, j)].content = player.ToString();

                    // Compute evaluation function for this move
                    int moveVal = MiniMax(grid, 0, false, squares);

                    //Undo the move
                    grid[(i, j)].content = " ";

                    //If the value of the current move is more than the best value,
                    //then update the best
                    if (moveVal > bestVal)
                    {
                        bestMove.row = j;
                        bestMove.col = i;
                        bestVal = moveVal;
                    }
                }
            }
        }

        Debug.Log($"Best move value {bestVal}");


        return bestMove;

    }





}
