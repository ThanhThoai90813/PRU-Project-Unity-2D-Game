using Inventory.Model;
using Inventory;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System.Collections;

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
    private GameObject player;
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private float lookAtPlayerRange = 5f;
    [SerializeField] 
    private bool canRotate = true;
    private Coroutine checkPlayerDistanceCoroutine;

    private void Start()
    {
        // Kiểm tra xem NPC này đã trao thưởng chưa khi khởi động
        hasReceivedReward = DBController.Instance.IsItemCollected(_npcToken);
        dialogueController.OnConversationEnded += GiveReward; // Đăng ký sự kiện
        player = GameObject.FindGameObjectWithTag("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Chạy Coroutine để kiểm tra khoảng cách Player
        checkPlayerDistanceCoroutine = StartCoroutine(CheckPlayerDistance());
    }
    public override void Interact()
    {
        Talk(dialogueText);
    }
    private IEnumerator CheckPlayerDistance()
    {
        while (true)
        {
            if (canRotate) LookAtPlayer();
            yield return new WaitForSeconds(0.2f);
        }
    }
    private void LookAtPlayer()
    {
        if (player == null || spriteRenderer == null) return;

        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance < lookAtPlayerRange)
        {
            spriteRenderer.flipX = player.transform.position.x < transform.position.x;
        }
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

        if (checkPlayerDistanceCoroutine != null)
        {
            StopCoroutine(checkPlayerDistanceCoroutine);
        }
    }
}
