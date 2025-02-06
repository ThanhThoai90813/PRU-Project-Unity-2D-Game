using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class ParallaxEffect : MonoBehaviour
{
    public Camera cam;
    public Transform followTarget;

    Vector2 startingPosition;

    float startingZ;

    Vector2 camMoveSinceStart => (Vector2)cam.transform.position - startingPosition;

    float zdistanceFromTarget => transform.position.z - followTarget.transform.position.z;

    float clippingPlane => (cam.transform.position.z + (zdistanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane));    
    float parallaxFactor => Mathf.Abs(zdistanceFromTarget) / clippingPlane;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startingPosition = transform.position;
        startingZ = transform.localPosition.z;
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newPosition = startingPosition + camMoveSinceStart * parallaxFactor;
        
        transform.position = new Vector3(newPosition.x, newPosition.y, startingZ);
    }
}
