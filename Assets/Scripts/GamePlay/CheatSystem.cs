using UnityEngine;
using System.Collections.Generic;
using System.Text;
using Inventory.Model;

public class CheatSystem : MonoBehaviour
{
    [SerializeField]
    private float cheatInputTimeout = 10f;

    private StringBuilder cheatInput = new StringBuilder(); // Chuỗi mã cheat người chơi nhập
    private float timer; // Đếm thời gian nhập cheat
    private bool isCheatActive = false; // Trạng thái cheat
    private bool isConsoleOpen = false; // Trạng thái console
    private string consoleInput = ""; // Chuỗi nhập trong console
    private List<string> consoleLog = new List<string>(); // Lưu log để hiển thị trên console
    private Vector2 scrollPosition; // Vị trí cuộn của log trong console
    private CursorLockMode previousLockState; // Lưu trạng thái khóa trước khi mở console
    private bool previousVisibility; // Lưu trạng thái hiển thị trước khi mở console
    // Danh sách mã cheat và hành động tương ứng
    private Dictionary<string, System.Action> cheatCodes = new Dictionary<string, System.Action>();

    void Start()
    {
        cheatCodes.Add("thanhthoaideptraibodoiqua", EnableThoaiDepTraiCheat);
        cheatCodes.Add("esp", CloseConsole);
        cheatCodes.Add("choanhtatca", GrantFullItems);
        timer = cheatInputTimeout;
    }
    private void CloseConsole()
    {
        ToggleConsole();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote)) // Phím "~" hoặc Esc
        {
            ToggleConsole();
        }

        if (isConsoleOpen && !isCheatActive)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                LogToConsole("Cheat input time expired!");
                isCheatActive = true;
                return;
            }
        }
    }

    void OnGUI()
    {
        if (!isConsoleOpen) return;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GUI.Box(new Rect(0, 0, Screen.width, Screen.height / 2), "");

        scrollPosition = GUI.BeginScrollView(new Rect(10, 10, Screen.width - 20, Screen.height / 2 - 60), scrollPosition, new Rect(0, 0, Screen.width - 40, consoleLog.Count * 20));
        for (int i = 0; i < consoleLog.Count; i++)
        {
            GUI.Label(new Rect(0, i * 20, Screen.width - 40, 20), consoleLog[i]);
        }
        GUI.EndScrollView();
        GUI.SetNextControlName("ConsoleInput");
        consoleInput = GUI.TextField(new Rect(10, Screen.height / 2 - 40, Screen.width - 20, 30), consoleInput);
        GUI.FocusControl("ConsoleInput");

        if (Event.current.keyCode == KeyCode.Return && consoleInput.Length > 0)
        {
            ProcessConsoleInput(consoleInput);
            consoleInput = ""; 
        }
    }

    private void ToggleConsole()
    {
        isConsoleOpen = !isConsoleOpen;
        if (isConsoleOpen)
        {
            previousLockState = Cursor.lockState;
            previousVisibility = Cursor.visible;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            LogToConsole("Console opened. Enter cheat code:");
            timer = cheatInputTimeout;
            isCheatActive = false;
        }
        else
        {
            Cursor.lockState = previousLockState;
            Cursor.visible = previousVisibility;

            LogToConsole("Console closed.");
            consoleInput = "";
            cheatInput.Clear();
        }
    }

    private void ProcessConsoleInput(string input)
    {
        LogToConsole("Input: " + input);
        cheatInput.Clear();
        cheatInput.Append(input);
        CheckCheatCode();
    }

    private void CheckCheatCode()
    {
        string code = cheatInput.ToString().ToLower();
        if (cheatCodes.ContainsKey(code))
        {
            LogToConsole("Cheat activated: " + code);
            cheatCodes[code].Invoke(); 
            isCheatActive = true; 
        }
        else
        {
            LogToConsole("Invalid cheat code: " + code);
            cheatInput.Clear();
        }
    }

    private void LogToConsole(string message)
    {
        consoleLog.Add(message);
        Debug.Log(message);
        scrollPosition.y = Mathf.Infinity;
    }
    private void EnableThoaiDepTraiCheat()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            Damageable damageable = player.GetComponent<Damageable>();
            Attack[] attacks = player.GetComponentsInChildren<Attack>();

            if (playerController != null && damageable != null)
            {
                // Bật chế độ bất tử vĩnh viễn từ cheat
                damageable.SetCheatInvincible(true);

                // Tăng gấp đôi tốc độ
                playerController.walkSpeed *= 2f;
                playerController.runSpeed *= 2f;
                playerController.airWalkSpeed *= 2f;

                // Tăng gấp đôi lực nhảy
                playerController.jumpImpulse *= 2f;

                LogToConsole("GOD MODE");
            }

            if (attacks != null && attacks.Length > 0)
            {
                // Tăng gấp 100 lần sát thương cho tất cả đòn đánh
                foreach (Attack attack in attacks)
                {
                    attack.baseAttackDamage *= 100;
                }
            }
            else
            {
                LogToConsole("No Attack components found on the player or its children.");
            }
        }
        else
        {
            LogToConsole("Player not found!");
        }
    }
    private void GrantFullItems()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PickUpSystem pickUpSystem = player.GetComponent<PickUpSystem>();
            if (pickUpSystem != null)
            {
                InventorySO inventory = pickUpSystem.GetType()
                    .GetField("inventoryData", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    .GetValue(pickUpSystem) as InventorySO;

                if (inventory != null)
                {
                    List<EdibleItemSO> edibleItems = EdibleManager.Instance.edibleItem;

                    foreach (EdibleItemSO item in edibleItems)
                    {
                        int remainder = inventory.AddItem(item, item.MaxStackSize);
                        if (remainder > 0)
                        {
                            LogToConsole($"Could not add full stack of {item.Name}. Remaining: {remainder}");
                        }
                        else
                        {
                            LogToConsole($"Added {item.MaxStackSize} of {item.Name} to inventory.");
                        }
                    }

                    LogToConsole("Full Items Cheat activated! All edible items added to inventory.");
                }
                else
                {
                    LogToConsole("InventorySO not found on player!");
                }
            }
            else
            {
                LogToConsole("PickUpSystem component not found on player!");
            }
        }
        else
        {
            LogToConsole("Player not found!");
        }
    }
}