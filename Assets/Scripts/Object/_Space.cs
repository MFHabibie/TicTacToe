using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TicTacToe.Object.Space
{
    /// <summary>
    /// Class represent the space on the board.
    /// </summary>
    public class _Space : MonoBehaviour
    {
        [SerializeField]
        private Button SpaceButton;

        [SerializeField]
        private TextMeshProUGUI SpaceText;

        /// <summary>
        /// Initialize Space
        /// </summary>
        private void Start()
        {
            SpaceButton.onClick.AddListener(SpaceClicked);
        }

        /// <summary>
        /// Listener function for space button when clicked
        /// </summary>
        private void SpaceClicked()
        {
            Set("O");
        }

        /// <summary>
        /// Set text value for the space and disable the button.
        /// </summary>
        /// <param name="Value"> string for setup the text. </param>
        public void Set(string Value) { 
            SpaceText.text = Value;
            SpaceButton.interactable = false;
        }

        /// <summary>
        /// Get the value of the space by returning string.
        /// </summary>
        public string Value()
        {
            return SpaceText.text;
        }

        /// <summary>
        /// Reset space back to default state
        /// </summary>
        public void ResetSpace()
        {
            SpaceText.text = "";
            SpaceButton.interactable = true;
        }
    }
}
