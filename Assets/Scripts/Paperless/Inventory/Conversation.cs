using UnityEngine;
using TMPro;

namespace GGJ2025_Paperless.Assets.Scripts.Paperless.Inventory
{
    /// Example:
    /// "I grab a coffee at the coffee shop"
    /// Coffee and coffee shop are keywords
    public class Conversation : MonoBehaviour
    {
        public Keyword keyword;

        public TextMeshProUGUI textElement; // Assign your TextMeshPro object in the Inspector
        private string template = "This is a part of an <color=red><b>{0}</b></color> word piece.";

        void Start()
        {
            string replacementWord = keyword.keywordString; // Replace this with your dynamic word
            textElement.text = string.Format(template, replacementWord);
        }
    }
}