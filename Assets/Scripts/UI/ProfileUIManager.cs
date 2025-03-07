using UnityEngine;
using TMPro;

public class ProfileUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textHealth;
    [SerializeField] private TextMeshProUGUI textPosition;
    [SerializeField] private TextMeshProUGUI textScene;
    [SerializeField] private TextMeshProUGUI textSaveDate;

    private void Start()
    {
        UpdateProfileUI();
    }

    public void UpdateProfileUI()
    {
        if (DBController.Instance != null)
        {
            textHealth.text = "Health: " + DBController.Instance.PLAYERHEALTH;
            textPosition.text = "Position: " + DBController.Instance.PLAYER_POSITION.ToString();
            textScene.text = "Scene: " + DBController.Instance.CURRENTSCENE;
            textSaveDate.text = "Last Save: " + DBController.Instance.SAVEDATETIME;
        }
        else
        {
            Debug.LogError("DBController is not initialized!");
        }
    }
}
