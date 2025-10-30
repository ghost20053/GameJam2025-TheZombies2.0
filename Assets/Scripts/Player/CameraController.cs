using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    public float mouseSensitivity = 3.0f;

    private float rotationY;
    private float rotationX;

    [SerializeField]
    public Transform target;

    [SerializeField]
    public float distanceFromTarget = 3.0f;

    public Vector3 currentRotation;
    public Vector3 smoothVelocity = Vector3.zero;

    [SerializeField]
    public float smoothTime = 0.2f;

    [SerializeField]
    public Vector2 rotationXMinMax = new Vector2(-40, 40);

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        rotationY += mouseX;
        rotationX += mouseY;

        //Apply clamping for x rotation
        rotationX = Mathf.Clamp(rotationX, rotationXMinMax.x, rotationXMinMax.y);

        Vector3 nextRotation = new Vector3(rotationX, rotationY);

        //Apply damping between rotation changes
        currentRotation = Vector3.SmoothDamp(currentRotation, nextRotation, ref smoothVelocity, smoothTime);
        transform.localEulerAngles = currentRotation;

        //Subtract forward vector of the GameObject to point its forward vector to the target
        transform.position = target.position - transform.forward * distanceFromTarget;
    }
}
