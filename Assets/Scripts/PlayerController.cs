using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    InputAction _R2Trigger, _R1Shoulder, rightAnalog, _RightAnalogClick;

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
    bool _isTilting;
    bool _isAnalogClicked = false;
    float _lowerPosition;
    [SerializeField]
    float _upperPosition;
    Vector3 _startRotation;
    [SerializeField]
    float _endRotationZ;

    [SerializeField]
    private float _elevationSpeed;
    [SerializeField]
    private float _rotationSpeed;

    void Start()
    {
        _R2Trigger.Enable();
        _R1Shoulder.Enable();
        rightAnalog.Enable();
        _RightAnalogClick.Enable();
        anim = hand.GetComponent<Animator>();
        _lowerPosition = gameObject.transform.position.y;
        _startRotation = hand.transform.rotation.eulerAngles;

    }

    void Update()
    {
        if (_R2Trigger.WasPressedThisFrame())
        {
            PickObject();
            anim.Play("Grab");
            playerMat.color = Color.red;
        }
        else if (_R2Trigger.WasReleasedThisFrame())
        {
            ReleaseObject();
            anim.Play("Release");
            playerMat.color = Color.wheat;
        }

        if (_R1Shoulder.WasPressedThisFrame())
        {
            _isTilting = true;
            Debug.Log("Tilting...");
        }
        else if (_R1Shoulder.WasReleasedThisFrame())
        {
            _isTilting = false;
            Debug.Log("Reverse Tilting...");
        }

        if (_RightAnalogClick.WasPressedThisFrame())
        {
            ElevationChange();
            Debug.Log("Elevation...");
        }

        TiltItem();
        Elevation();

        Vector2 analogInput = rightAnalog.ReadValue<Vector2>();

        if (analogInput.magnitude > 0.1f) // Deadzone   
        {
            float magnitude = analogInput.magnitude;
            float angle = Mathf.Atan2(analogInput.y, analogInput.x) * Mathf.Rad2Deg;

            //Debug.Log($"Analog - X: {analogInput.x:F3}, Y: {analogInput.y:F3}, Magnitude: {magnitude:F3}, Angle: {angle:F1}°");
        }

        rb.AddForce(new Vector3(analogInput.x * Time.deltaTime * movementSpeed, 0, analogInput.y * Time.deltaTime * movementSpeed), ForceMode.Impulse);
        gameObject.transform.Translate(new Vector3(analogInput.x * Time.deltaTime * movementSpeed, 0, analogInput.y * Time.deltaTime * movementSpeed));
    }

    private void ElevationChange()
    {
        _isAnalogClicked = !_isAnalogClicked;
    }

    private void Elevation()
    {
        if (_isAnalogClicked)
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, new Vector3(gameObject.transform.position.x, _upperPosition, gameObject.transform.position.z), _elevationSpeed * Time.deltaTime);
        if (!_isAnalogClicked)
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, new Vector3(gameObject.transform.position.x, _lowerPosition, gameObject.transform.position.z), _elevationSpeed * Time.deltaTime);
    }

    private void TiltItem()
    {
        if (_isTilting)
            hand.transform.rotation = Quaternion.RotateTowards(hand.transform.rotation, Quaternion.Euler(_startRotation.x, _startRotation.y, _endRotationZ), _rotationSpeed * Time.deltaTime);
        if (!_isTilting)
            hand.transform.rotation = Quaternion.RotateTowards(hand.transform.rotation, Quaternion.Euler(_startRotation), _rotationSpeed * Time.deltaTime);

    }

    private void ReleaseObject()
    {
        _isGrabbing = false;

        if (_item)
        {
            _item.gameObject.transform.parent = null;
            _item.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            _item = null;
        }



    }

    private void PickObject()
    {
        _isGrabbing = true;
        RaycastHit hit;
        if (Physics.SphereCast(gameObject.transform.position, 0.1f, transform.TransformDirection(Vector3.left), out hit, 1f, LayerMask.GetMask("Item")))
        {
            _item = hit.transform.gameObject;
            Debug.Log("Found ITEM!");
            playerMat.color = Color.green;
        }

        Debug.DrawRay(gameObject.transform.position, transform.TransformDirection(Vector3.left), Color.red);

        if (_item)
        {
            _item.gameObject.transform.parent = hand.transform;
            _item.gameObject.GetComponent<Rigidbody>().isKinematic = true;
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
