using UnityEngine;
using UnityEngine.SceneManagement;
using ToyBox.InputSystem;
using UnityEngine.InputSystem;

public class ReadyUpHandler : MonoBehaviour
{
    private PlayerInputSystem inputSystem;
    [SerializeField] private bool isReady = false;
    [SerializeField] private bool canReturnToLobby = false;

    void Awake()
    {
        inputSystem = GetComponent<PlayerInputSystem>();
    }

    void OnEnable()
    {
        inputSystem.OnValidateReadyEvent.Performed += OnReadyPressed;
    }

    void OnDisable()
    {
        inputSystem.OnValidateReadyEvent.Performed -= OnReadyPressed;
    }
    void OnReadyPressed()
    {
        if (!canReturnToLobby)
        {
            Debug.Log("Retour au lobby désactivé pour le moment.");
            return;
        }

        if (isReady)
        {
            Debug.Log($"{gameObject.name} n'est plus prêt !");
            isReady = false;
            ReadyManager.Instance.PlayerSetReady(this);
            return;
        };

        isReady = true;
        Debug.Log($"{gameObject.name} est prêt !");
        ReadyManager.Instance.PlayerSetReady(this);
    }

    
    public void EnableLobbyReturn()
    {
        canReturnToLobby = true;
    }
    
    public bool CheckReady()
    {
        return isReady;
    }
    
    public void ResetReady()
    {
        isReady = false;
        canReturnToLobby = false;
    }

    public bool IsReady() => isReady;
}
