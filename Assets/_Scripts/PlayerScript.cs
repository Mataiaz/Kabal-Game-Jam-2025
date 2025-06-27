using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacter : MonoBehaviour
{
    public GameObject characterCamera;
    private Rigidbody rb;
    private Vector3 movementInput;
    
    private float cameraPitch = 0f;
    public float mouseSensitivity = 2f;

void Start() {
    rb = GetComponent<Rigidbody>();
}


    private void Update()
    {

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -80f, 80f);
        characterCamera.transform.localEulerAngles = new Vector3(cameraPitch, 0f, 0f);

        Vector3 camForward = characterCamera.transform.forward;
        Vector3 camRight = characterCamera.transform.right;

        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        movementInput = (camForward * moveZ + camRight * moveX).normalized;
        rb.MovePosition(transform.position + movementInput * Time.deltaTime * 5f);
    
}


    private void FixedUpdate() {
        Movement();
    }

    private void Movement() {
        if (rb != null)
            rb.MovePosition(transform.position + movementInput * Time.deltaTime * 5f);
    }

}