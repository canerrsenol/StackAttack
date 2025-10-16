using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    [SerializeField] private float moveDistance = 5f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private Vector3 direction = Vector3.back;

    private Vector3 startPosition;
    private Vector3 moveDirection;

    private void Start()
    {
        startPosition = transform.position;
        moveDirection = direction.sqrMagnitude > 0f ? direction.normalized : Vector3.forward;
    }

    private void Update()
    {
        float distance = Mathf.PingPong(Time.time * speed, moveDistance);
        transform.position = startPosition + moveDirection * distance;
    }
}