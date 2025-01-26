using UnityEngine;

namespace GGJ2025_Paperless.Assets.Scripts.Paperless.Inventory
{
    public class Inventory : MonoBehaviour
    {
        private static Inventory _instance;
        public static Inventory Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<Inventory>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("Inventory");
                        _instance = go.AddComponent<Inventory>();
                    }
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
        }

        public InventorySlot[] slots = new InventorySlot[2];

        public InventorySlot GetSlotAt(int index)
        {
            if (index < 0 || index >= slots.Length)
            {
                Debug.LogWarning($"Attempted to get invalid inventory slot at index {index}");
                return null;
            }
            return slots[index];
        }

        public bool SetSlotAt(int index, Keyword keyword)
        {
            if (index < 0 || index >= slots.Length)
            {
                Debug.LogWarning($"Attempted to set invalid inventory slot at index {index}");
                return false;
            }
            slots[index]?.SetKeyword(keyword);
            return true;
        }
    }
}