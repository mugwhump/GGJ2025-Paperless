using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
namespace GGJ2025_Paperless.Assets.Scripts.Paperless.Inventory
{
    public class Conversation : MonoBehaviour
    {
        private static Conversation _instance;
        public static Conversation Instance
        {
            get
            {
                if (_instance == null) _instance = FindFirstObjectByType<Conversation>();
                return _instance;
            }
        }

        public IKeywordSource keywordSource;
        public TextMeshProUGUI textElement; // Assign your TextMeshPro object in the Inspector
        private bool keywordExchanged = false; // Flag to track if the keyword has been exchanged

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
        }

        void Start()
        {
            UpdateText();
        }

        void Update()
        {
            if (gameObject.activeSelf)
            {
                TakeKeyword();
            }
        }

        [Button]
        public void Enable(Vector3 position, IKeywordSource keywordSource)
        {
            gameObject.SetActive(true);
            transform.position = position + new Vector3(-1f, 3f, 0);
            UpdateText();
            this.keywordSource = keywordSource;
        }

        [Button]
        public void Disable() 
        {
            gameObject.SetActive(false);
        }

        [Button]
        private void UpdateText()
        {
            textElement.text = string.Format(
                keywordSource?.GetDialogueTemplate() ?? "Invalid template: {0}",
                keywordSource?.GetKeyword()?.keywordString ?? "_____");
        }

        private void TakeKeyword()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                int slotIndex = -1;
                if (Input.GetKeyDown(KeyCode.K))
                {
                    slotIndex = 0;
                }
                else if (Input.GetKeyDown(KeyCode.L))
                {
                    slotIndex = 1;
                }

                if (slotIndex != -1 && !keywordExchanged)
                {
                    var currentKeyword = Inventory.Instance.GetSlotAt(slotIndex).keyword;
                    Inventory.Instance.SetSlotAt(slotIndex, keywordSource?.GetKeyword());
                    keywordSource?.SetKeyword(currentKeyword);
                    keywordExchanged = true; 
                }
            }
            else
            {
                keywordExchanged = false; // Reset the flag when the spacebar is released
            }
        }
    }

}