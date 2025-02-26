using TMPro;
using UnityEngine;

public class TextDescriptionTxtController : MonoBehaviour
{
    public TMP_Text descriptionText;
    public AudioSource pageTurnSound;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))  
        {
            NextPage();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) 
        {
            PreviousPage();
        }
    }
    private void NextPage()
    {
        descriptionText.pageToDisplay++;
        PlaySound();
    }

    private void PreviousPage()
    {
        descriptionText.pageToDisplay = Mathf.Max(1, descriptionText.pageToDisplay - 1);
        PlaySound();
    }

    private void PlaySound()
    {
        if (pageTurnSound != null)
        {
            pageTurnSound.Play();
        }
    }
}
