using UnityEngine;
using UnityEngine.InputSystem;

public abstract class NPC : MonoBehaviour, IInteractable
{
    [SerializeField]
    private SpriteRenderer _interactSprite;
    private Transform _playerTransform;
    private const float INTERACT_DISTANCE = 5f;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            _playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Không tìm thấy Player! Hãy đảm bảo rằng Player có tag 'Player'.");
        }
    }

    private void Update()
    {
        if (_playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) _playerTransform = player.transform;
        }

        if (Keyboard.current.eKey.wasPressedThisFrame && IsWithinInteractDistance())
        {
            Interact();
        }

        if (_interactSprite != null)
        {
            if (_interactSprite.gameObject.activeSelf && !IsWithinInteractDistance())
            {
                _interactSprite.gameObject.SetActive(false);
            }
            else if (!_interactSprite.gameObject.activeSelf && IsWithinInteractDistance())
            {
                _interactSprite.gameObject.SetActive(true);
            }
        }
        else
        {
            Debug.LogError("SpriteRenderer chưa được gán trong Inspector!");
        }
    }

    public abstract void Interact();

    private bool IsWithinInteractDistance()
    {
        if (_playerTransform == null) return false;
        return Vector2.Distance(_playerTransform.position, transform.position) < INTERACT_DISTANCE;
    }
}
