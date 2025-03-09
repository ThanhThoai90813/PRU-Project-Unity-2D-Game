using Inventory.Model;
using Inventory;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Trader1 : NPC, ITalkable
{
    [SerializeField]
    private DialogueText dialogueText;

    [SerializeField]
    private DialogueController dialogueController;
    
    [SerializeField] 
    private InventoryController playerInventory;
    
    [SerializeField] 
    private List<ItemSO> rewardItems;
    
    [SerializeField]
    private List<int> rewardQuantities;

    [SerializeField]
    private int _npcToken;

    [SerializeField]
    private UINotification notificationUI;

    private bool hasReceivedReward = false;
   
    private void Start()
    {
        // Kiểm tra xem NPC này đã trao thưởng chưa khi khởi động
        hasReceivedReward = DBController.Instance.IsItemCollected(_npcToken);
        dialogueController.OnConversationEnded += GiveReward; // Đăng ký sự kiện
    }
    public override void Interact()
    {
        Talk(dialogueText);
    }

    public void Talk(DialogueText dialogueText)
    {
        //Start Convertation
        dialogueController.DisplayNextParagraph(dialogueText);
    }

    private void GiveReward()
    {
        if (hasReceivedReward)
        {
            Debug.Log($"NPC {_npcToken} đã trao thưởng trước đó, không trao lại.");
            return;
        }

        if (playerInventory == null)
        {
            return;
        }

        if (rewardItems.Count != rewardQuantities.Count)
        {
            return;
        }

        for (int i = 0; i < rewardItems.Count; i++)
        {
            playerInventory.inventoryData.AddItem(rewardItems[i], rewardQuantities[i]);
            string message = $"Get: {rewardItems[i].name}, quantity: {rewardQuantities[i]}"; Debug.Log(message);
            Debug.Log(message);
            if (notificationUI != null)
            {
                notificationUI.ShowNotification(message);
            }
            else
            {
                Debug.LogWarning("NotificationUI chưa được gán trong Inspector!");
            }
        }

        hasReceivedReward = true;
        DBController.Instance.AddCollectedToken(_npcToken);
    }

    private void OnDestroy()
    {
        dialogueController.OnConversationEnded -= GiveReward;
    }
}
