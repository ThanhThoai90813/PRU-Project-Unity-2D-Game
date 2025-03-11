using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;

public class CameraReassign : MonoBehaviour
{
    private CinemachineCamera cinemachineCamera;

    private void Start()
    {
        cinemachineCamera = GetComponent<CinemachineCamera>();
        if (cinemachineCamera == null)
        {
            Debug.LogError("CinemachineCamera component not found on this GameObject!");
            return;
        }
        AssignPlayer();
    }

    private void AssignPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("⚠ Player chưa được gán trong CameraReassign!");
            return;
        }

        if (cinemachineCamera != null)
        {
            cinemachineCamera.Follow = player.transform;
        }
        else
        {
            Debug.LogWarning("⚠ CameraFollow chưa được gán!");
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignPlayer();
    }
}