using Lean.Touch;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float forwardSpeed = 5f;
    [SerializeField] private float horizontalSpeed = 5f;
    [SerializeField] private float borderLimit = 10f;
    [SerializeField] private float swipeDeadzone = 10f;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GlobalEventsSO globalEventsSO;
    private bool isTouching = false;
    private GameManager gameManager;

    void Awake()
    {
        gameManager = GameManager.Instance;
    }

    void OnEnable()
    {
        LeanTouch.OnFingerUpdate += HandleFingerUpdate;
        LeanTouch.OnFingerDown += (finger) => { isTouching = true; };
        LeanTouch.OnFingerUp += (finger) => { isTouching = false; };
    }

    void OnDisable()
    {
        LeanTouch.OnFingerUpdate -= HandleFingerUpdate;
        LeanTouch.OnFingerDown -= (finger) => { isTouching = true; };
        LeanTouch.OnFingerUp -= (finger) => { isTouching = false; };
    }

    private void HandleFingerUpdate(LeanFinger finger)
    {
        if (gameManager.GameState != GameState.Started) return;

        if (!isTouching) return;
        if (finger.IsOverGui) return;

        Vector2 delta = finger.ScaledDelta;

        if (delta.magnitude < swipeDeadzone)
        {
            return;
        }

        Vector3 move = new Vector3(delta.x, 0f, 0f) * horizontalSpeed * Time.deltaTime;

        Vector3 newPosition = playerTransform.localPosition + move;
        newPosition.x = Mathf.Clamp(newPosition.x, -borderLimit, borderLimit);
        newPosition.z = Mathf.Clamp(newPosition.z, -borderLimit, borderLimit);

        playerTransform.localPosition = newPosition;
    }
    
    private void Update()
    {
        if (gameManager.GameState != GameState.Started) return;
        transform.position += Vector3.forward * forwardSpeed * Time.deltaTime;
        globalEventsSO.PlayerEvents.zPositionChanged?.Invoke(transform.position.z);
    }
}