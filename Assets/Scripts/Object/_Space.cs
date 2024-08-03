using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TicTacToe.Object.Board;

namespace TicTacToe.Object.Space
{
    /// <summary>
    /// Class represent the space on the board.
    /// </summary>
    public class _Space : MonoBehaviour
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        [SerializeField]
        private int coordX;
        [SerializeField]
        private int coordY;

        private _Board board;
        private Button spaceButton;
        private TextMeshProUGUI spaceText;

        /// <summary>
        /// Setup first time on runtime
        /// </summary>
        private void Start()
        {
            X = coordX;
            Y = coordY;

            spaceButton = GetComponent<Button>();
            spaceButton.onClick.AddListener(SpaceClicked);

            spaceText=GetComponentInChildren<TextMeshProUGUI>();
        }

        /// <summary>
        /// Initialize space with set the board reference
        /// </summary>
        /// <param name="inBoard">Object represent as a Board</param>
        public void SetupSpace(_Board inBoard)
        {
            board = inBoard;
        }

        /// <summary>
        /// Listener function for space button when clicked
        /// </summary>
        private void SpaceClicked()
        {
            Set(board.PlayerState());
            board.MoveStep(this);
        }

        /// <summary>
        /// Set text value for the space and disable the button.
        /// </summary>
        /// <param name="Value"> string for setup the text. </param>
        public void Set(string Value) { 
            spaceText.text = Value;
            spaceButton.interactable = false;
        }

        /// <summary>
        /// Get the value of the space by returning string.
        /// </summary>
        public string Value()
        {
            return spaceText.text;
        }

        /// <summary>
        /// Lock the space by disable the interactable button
        /// </summary>
        public void LockSpace()
        {
            spaceButton.interactable = false;
        }

        /// <summary>
        /// Reset space back to default state
        /// </summary>
        public void ResetSpace()
        {
            spaceText.text = "";
            spaceButton.interactable = true;
        }
    }
}
