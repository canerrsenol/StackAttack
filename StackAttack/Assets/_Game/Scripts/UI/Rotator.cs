using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 100f;

    // Update is called once per frame
    void Update()
    {
        // Z ekseninde objeyi döndürür.
        transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
    }
}
