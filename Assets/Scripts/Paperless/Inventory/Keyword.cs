using UnityEngine;

namespace GGJ2025_Paperless.Assets.Scripts.Paperless.Inventory
{
    [CreateAssetMenu(fileName = "Keyword", menuName = "Keyword", order = 0)]
    public class Keyword : ScriptableObject
    {
        public string keywordString;
        public Sprite icon;
    }
}