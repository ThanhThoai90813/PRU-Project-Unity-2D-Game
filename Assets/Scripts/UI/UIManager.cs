using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public GameObject damageTextPrefab; // Vẫn giữ để hiển thị số khi bị damage
    public GameObject healthSpritePrefab; // Thay healthImagePrefab bằng healthSpritePrefab

    public Canvas gameCanvas; // Vẫn cần cho damageTextPrefab (UI)

    private void Awake()
    {
        gameCanvas = FindAnyObjectByType<Canvas>();
    }

    private void OnEnable()
    {
        CharacterEvents.characterDamaged += CharacterTookDamage;
        CharacterEvents.characterHealed += CharacterHealed;
    }

    private void OnDisable()
    {
        CharacterEvents.characterDamaged -= CharacterTookDamage;
        CharacterEvents.characterHealed -= CharacterHealed;
    }

    public void CharacterTookDamage(GameObject character, int damageReceived)
    {
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(character.transform.position);

        TMP_Text tmpText = Instantiate(damageTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform)
            .GetComponent<TMP_Text>();

        tmpText.text = damageReceived.ToString();
    }

    public void CharacterHealed(GameObject character, int healthRestored)
    {
        // Đặt vị trí ban đầu trong world-space (không cần Canvas)
        Vector3 spawnPosition = character.transform.position + new Vector3(0, 1f, 0); // Đặt phía trên nhân vật

        // Instantiate healthSpritePrefab
        GameObject healthSpriteObject = Instantiate(healthSpritePrefab, spawnPosition, Quaternion.identity);

        // Điều chỉnh scale dựa trên lượng máu hồi (tùy chọn)
        float scale = Mathf.Clamp(healthRestored / 5f, 1f, 3f);
        healthSpriteObject.transform.localScale = new Vector3(scale, scale, 1);
    }

    public void OnExitGame(InputAction.CallbackContext context)
    {
        if (context.started)
        {
#if (UNITY_EDITOR || DEVELOPMENT_BUILD)
            Debug.Log(this.name + " : " + this.GetType() + " : " + System.Reflection.MethodBase.GetCurrentMethod().Name);
#endif

#if (UNITY_EDITOR)
            UnityEditor.EditorApplication.isPlaying = false;
#elif (UNITY_STANDALONE)
            Application.Quit();
#elif (UNITY_WEBGL)
            SceneManager.LoadScene("QuitScene");
#endif
        }
    }
}