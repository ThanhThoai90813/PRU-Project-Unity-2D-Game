using UnityEngine;

public class BuffUIManager : MonoBehaviour
{
    public static BuffUIManager Instance { get; private set; }

    [SerializeField] private GameObject buffUIPrefab;
    [SerializeField] private Transform buffUIContainer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddBuffUI(BuffDataSO buffData, float duration)
    {
        if (buffUIPrefab == null || buffUIContainer == null)
        {
            return;
        }

        GameObject buffUIObject = Instantiate(buffUIPrefab, buffUIContainer);
        BuffUIElement buffUI = buffUIObject.GetComponent<BuffUIElement>();
        buffUI.Initialize(buffData, duration);
    }
}