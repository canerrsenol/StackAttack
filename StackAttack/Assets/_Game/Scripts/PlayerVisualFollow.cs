using UnityEngine;

public class PlayerVisualFollow : MonoBehaviour
{
    [SerializeField] private Transform targetTransform; // playerTransform atanacak
    [SerializeField] private float followSpeed = 8f;
    [SerializeField] private float tiltAmount = 20f;
    private float currentTilt = 0f;
    private float velocity = 0f;

    void Update()
    {
        if (targetTransform == null) return;

        // X pozisyonunu gecikmeli takip et
        float newX = Mathf.SmoothDamp(transform.localPosition.x, targetTransform.localPosition.x, ref velocity, 1f / followSpeed);
        Vector3 newPos = transform.localPosition;
        newPos.x = newX;
        transform.localPosition = newPos;

        // Hedef ile aradaki x farkına göre eğilme
        float xDiff = targetTransform.localPosition.x - transform.localPosition.x;
        float targetTilt = -Mathf.Clamp(xDiff, -1f, 1f) * tiltAmount;
        currentTilt = Mathf.Lerp(currentTilt, targetTilt, Time.deltaTime * followSpeed);
        // Aynı xDiff değerinden hesaplanan eğilmeyi hem Y hem Z eksenine uygula
        transform.localRotation = Quaternion.Euler(0f, -currentTilt, currentTilt);
    }
}
