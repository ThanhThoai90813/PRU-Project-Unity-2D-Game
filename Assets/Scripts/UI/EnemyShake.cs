using UnityEngine;
using Unity.Cinemachine;

public class EnemyShake : MonoBehaviour
{
    public CinemachineImpulseSource impulseSource;

    public void ShakeCamera()
    {
        if (impulseSource != null)
        {
            impulseSource.GenerateImpulse();
        }
    }
}
