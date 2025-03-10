using System.Collections;
using UnityEngine;

public class SceneFreeze : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(FreezeGameAfterLoad());
    }

    private IEnumerator FreezeGameAfterLoad()
    {
        Time.timeScale = 0f; 
        yield return new WaitForSecondsRealtime(1.5f); 
        Time.timeScale = 1f;
    }
}
