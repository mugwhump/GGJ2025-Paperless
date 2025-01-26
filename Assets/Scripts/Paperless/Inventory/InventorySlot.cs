using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace GGJ2025_Paperless.Assets.Scripts.Paperless.Inventory
{
    public class InventorySlot : MonoBehaviour
    {
        public Keyword keyword;
        public Image image;
        public Image borderImage;
    
        [Button]
        public void SetKeyword(Keyword newKeyword)
        {
            keyword = newKeyword;
            image.sprite = newKeyword ? newKeyword.icon : null;
            image.enabled = newKeyword != null && newKeyword.icon != null;
        }
    }
}