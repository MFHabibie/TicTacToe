using System;
using System.Collections;
using System.Collections.Generic;
using TicTacToe.Global.GameController;
using TicTacToe.Object.Space;
using TicTacToe.Utility.ServiceLocator;
using UnityEngine;

namespace TicTacToe.Object.Board
{
    public class _Board : MonoBehaviour
    {
        public State[,] Board { get; private set; }
        public int Size { get; private set; }

        private IGameController GameController;

        private State lastPlayerState;

        [SerializeField]
        private _Space[] spaces;

        private void Awake()
        {
            _ServiceLocator.Global.Register<IGameController>(GameController = new _GameController());
        }

        private void Start()
        {
            _ServiceLocator.For(this).Get(out GameController);
            SetupBoard();
        }

        private void SetupBoard()
        {
            Size = Convert.ToInt32(Mathf.Sqrt(spaces.Length));
            Board = new State[Size, Size];
            GameController.SetupBoard(this);

            for (int i = 0; i < spaces.Length; i++)
            {
                spaces[i].SetupSpace(this);
            }
        }

        public string PlayerState()
        {
            lastPlayerState = GameController.GetNextPlayerState();
            return lastPlayerState.ToString();
        }

        public void MoveStep(_Space space)
        {
            Board[space.X, space.Y] = lastPlayerState;
            GameController.CheckOnSteps(space.X, space.Y, lastPlayerState);
        }

        public void LockMovement()
        {
            for (int i = 0; i < spaces.Length; i++)
            {
                spaces[i].LockSpace();
            }
        }
    }
}
