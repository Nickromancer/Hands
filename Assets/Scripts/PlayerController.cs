using System;
using Unity.VisualScripting;
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
    GameObject hand, _item;

    [SerializeField]
    Collider _col;
    Animator anim;
    bool _isGrabbing;

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
            PickObject();
            anim.Play("Grab");
            playerMat.color = Color.red;
        }
        else if (trigger.WasReleasedThisFrame())
        {
            ReleaseObject();
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

    private void ReleaseObject()
    {
        _isGrabbing = false;

        _item.gameObject.transform.parent = null;

    }

    private void PickObject()
    {
        _isGrabbing = true;
        RaycastHit hit;
        if (Physics.Raycast(gameObject.transform.position, transform.TransformDirection(Vector3.down), out hit, LayerMask.GetMask("Item")))
        {
            _item = hit.transform.gameObject;
        }

        if (_item)
        {
            _item.gameObject.transform.parent = gameObject.transform;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (_isGrabbing && _item == null)
        {
            _item = collision.gameObject;
        }
        else if (!_isGrabbing)
        {
            _item = null;
        }
    }
}
