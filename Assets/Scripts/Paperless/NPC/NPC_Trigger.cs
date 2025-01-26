using System;
using GGJ2025_Paperless.Assets.Scripts.Paperless.Inventory;
using UnityEngine;

namespace Paperless.NPC
{
    [RequireComponent(typeof(NPC_Conversation))]
    public class NPC_Trigger : MonoBehaviour
    {
        [SerializeField]
        private BoxCollider2D conversationTrigger;

        [SerializeField]
        private NPC_Conversation conversation;

        void Start()
        {
            conversation = GetComponent<NPC_Conversation>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            conversation?.ShowDialogue();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            conversation?.HideDialogue();
        }
    }
}