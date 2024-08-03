using System.Collections;
using System.Collections.Generic;
using TicTacToe.Object.Board;
using TMPro;
using UnityEngine;

namespace TicTacToe.Global.GameController
{
    public enum State { Blank, X, O };
    public enum GameState { Draw, FirstPlayerWin, SecondPlayerWin };

    /// <summary>
    /// Represent the game controller.
    /// </summary>
    public class _GameController : MonoBehaviour, IGameController
    {
        private _Board board;
        private State lastPlayerIcon;
        private GameState gameState;
        private int moveCount;

        public void SetupBoard(_Board inBoard)
        {
            board = inBoard;
        }

        /// <summary>
        /// Get the icon for next player represent as a State enum
        /// </summary>
        public State GetNextPlayerState()
        {
            lastPlayerIcon = lastPlayerIcon == State.O ? State.X : State.O;
            return lastPlayerIcon;
        }

        /// <summary>
        /// Check whether the game could be finished or still could run
        /// </summary>
        /// <param name="coordX">Coordinate for X on selected space</param>
        /// <param name="coordY">Coordinate for Y on selected space</param>
        /// <param name="state">String value for the icon inside space represent as string</param>
        public void CheckOnSteps(int coordX, int coordY, State state)
        {
            moveCount++;

            //check whether state make align on column
            for (int i = 0; i < board.Size; i++)
            {
                if (board.Board[coordX, i] != state)
                {
                    break;
                }

                if (i == board.Size - 1)
                {
                    gameState = state == State.O ? GameState.FirstPlayerWin : GameState.SecondPlayerWin;
                    GameEnd();
                    return;
                }
            }

            //check whether state make align on row
            for (int i = 0; i < board.Size; i++)
            {
                if (board.Board[i, coordY] != state)
                {
                    break;
                }

                if (i == board.Size - 1)
                {
                    gameState = state == State.O ? GameState.FirstPlayerWin : GameState.SecondPlayerWin;
                    GameEnd();
                    return;
                }
            }

            //check whether state make align on diagonal
            if (coordX == coordY)
            {
                //we're on a diagonal
                for (int i = 0; i < board.Size; i++)
                {
                    if (board.Board[i, i] != state)
                    {
                        break;
                    }

                    if (i == board.Size - 1)
                    {
                        gameState = state == State.O ? GameState.FirstPlayerWin : GameState.SecondPlayerWin;
                        GameEnd();
                        return;
                    }
                }
            }

            //check whether state make align on anti diagonal
            if (coordX + coordY == board.Size - 1)
            {
                for (int i = 0; i < board.Size; i++)
                {
                    if (board.Board[i, (board.Size - 1) - i] != state)
                    {
                        break;
                    }

                    if (i == board.Size - 1)
                    {
                        gameState = state == State.O ? GameState.FirstPlayerWin : GameState.SecondPlayerWin;
                        GameEnd();
                        return;
                    }
                }
            }

            //check whether space is full so make it draw
            if (moveCount == (Mathf.Pow(board.Size, 2)))
            {
                gameState = GameState.Draw;
                GameEnd();
            }
        }

        /// <summary>
        /// Run end process of the game and get the result
        /// </summary>
        private void GameEnd()
        {
            board.LockMovement();

            switch (gameState)
            {
                case GameState.FirstPlayerWin:
                    Debug.Log("O Win");
                    break;
                case GameState.SecondPlayerWin:
                    Debug.Log("X Win");
                    break;
                case GameState.Draw:
                    Debug.Log("Draw");
                    break;
            }
        }
    }

    public interface IGameController
    {
        void SetupBoard(_Board board);
        State GetNextPlayerState();
        void CheckOnSteps(int coordX, int coordY, State state);
    }
}