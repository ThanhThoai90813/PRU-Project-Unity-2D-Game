using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuffUIElement : MonoBehaviour
{
    [SerializeField] private Image buffIcon;
    [SerializeField] private Image fillImage; 
    [SerializeField] private TextMeshProUGUI durationText;

    private float totalDuration;
    private float remainingDuration;
    private bool isBlinking;
    private const float BLINK_THRESHOLD = 3f; 
    private const float BLINK_SPEED = 0.5f;

    public void Initialize(BuffDataSO buffData, float duration)
    {
        totalDuration = duration;
        remainingDuration = duration;
        buffIcon.sprite = buffData.buffIcon;
        buffIcon.color = buffData.iconColor;
        UpdateUI();
    }

    private void Update()
    {
        if (remainingDuration > 0)
        {
            remainingDuration -= Time.deltaTime;
            UpdateUI();

            RotateFillImage();

            if (remainingDuration <= BLINK_THRESHOLD && !isBlinking)
            {
                StartBlinking();
            }

            if (remainingDuration <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void UpdateUI()
    {
        fillImage.fillAmount = remainingDuration / totalDuration;
        durationText.text = Mathf.Ceil(remainingDuration).ToString();
    }

    private void StartBlinking()
    {
        isBlinking = true;
        StartCoroutine(BlinkEffect());
    }
    private void RotateFillImage()
    {
        float progress = remainingDuration / totalDuration; 
        float rotationAngle = 360f * (1f - progress);
        fillImage.rectTransform.localRotation = Quaternion.Euler(0, 0, rotationAngle);
    }
    private System.Collections.IEnumerator BlinkEffect()
    {
        while (remainingDuration > 0)
        {
            buffIcon.color = new Color(buffIcon.color.r, buffIcon.color.g, buffIcon.color.b, 0.3f);
            yield return new WaitForSeconds(BLINK_SPEED);
            buffIcon.color = new Color(buffIcon.color.r, buffIcon.color.g, buffIcon.color.b, 1f);
            yield return new WaitForSeconds(BLINK_SPEED);
        }
    }
}