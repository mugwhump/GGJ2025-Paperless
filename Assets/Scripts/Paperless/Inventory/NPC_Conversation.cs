using Sirenix.OdinInspector;
using UnityEngine;

namespace GGJ2025_Paperless.Assets.Scripts.Paperless.Inventory
{
    public class NPC_Conversation : MonoBehaviour, IKeywordSource
    {
        public Keyword keyword;
        [SerializeField]
        private string template = "This is a part of an <color=red><b>{0}</b></color> word piece.";
        
        public Keyword GetKeyword()
        {
            return keyword;
        }

        public void SetKeyword(Keyword newKeyword)
        {
            keyword = newKeyword;
        }

        public string GetDialogueTemplate()
        {
            return template;
        }
        
        [Button]
        private void UpdateText()
        {
            Conversation.Instance.Enable(transform.position, this);
        }
    }
}