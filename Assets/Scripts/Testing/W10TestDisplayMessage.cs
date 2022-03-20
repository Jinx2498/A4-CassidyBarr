using GameBrains.Extensions.MonoBehaviours;
using GameBrains.Messages;
using UnityEngine;

namespace Testing
{
    public class W10TestDisplayMessage : ExtendedMonoBehaviour
    {
        [SerializeField] MessageQueue[] messageQueues;

        [SerializeField] bool sendMessages;
        int messageCount;

        public override void Update()
        {
            if (!sendMessages) { return; }
            
            sendMessages = false;
            messageCount++;

            foreach (var messageQueue in messageQueues)
            {
                messageQueue.AddMessage($"Test Message {messageCount}");
            }
        }
    }
}