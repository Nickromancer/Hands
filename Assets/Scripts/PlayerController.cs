using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    InputAction trigger, rightAnalog;

    [SerializeField]
    Material playerMat;

    [SerializeField]
    float movementSpeed = 10;

    [SerializeField]
    Rigidbody rb;

    [SerializeField]
    GameObject hand;
    Animator anim;
    void Start()
    {

        trigger.Enable();
        rightAnalog.Enable();
        anim = hand.GetComponent<Animator>();
    }

    void Update()
    {
        if (trigger.WasPressedThisFrame())
        {
            anim.Play("Grab");
            playerMat.color = Color.red;
        }
        else if (trigger.WasReleasedThisFrame())
        {
            anim.Play("Release");
            playerMat.color = Color.wheat;
        }

        Vector2 analogInput = rightAnalog.ReadValue<Vector2>();

        if (analogInput.magnitude > 0.1f) // Deadzone   
        {
            float magnitude = analogInput.magnitude;
            float angle = Mathf.Atan2(analogInput.y, analogInput.x) * Mathf.Rad2Deg;

            Debug.Log($"Analog - X: {analogInput.x:F3}, Y: {analogInput.y:F3}, Magnitude: {magnitude:F3}, Angle: {angle:F1}°");
        }

        rb.AddForce(new Vector3(analogInput.x * Time.deltaTime * movementSpeed, 0, analogInput.y * Time.deltaTime * movementSpeed), ForceMode.Impulse);
        gameObject.transform.Translate(new Vector3(analogInput.x * Time.deltaTime * movementSpeed, 0, analogInput.y * Time.deltaTime * movementSpeed));
    }
}
