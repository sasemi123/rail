using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem.iOS;

public class ScriptPlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 14f;
    [SerializeField] private Vector2 rangeX = new Vector2(-7f, 7f);
    [SerializeField] private Vector2 rangeY = new Vector2(2.5f, 9f);

    [Header("Rotation")]
    [SerializeField] private float pitchAmount = 20f;
    [SerializeField] private float yawAmount = 15f;
    [SerializeField] private float rollAmount = 35f;
    [SerializeField] private float tiltSmooth = 8f;

    private Vector3 targetLocalPosition;

    private void Start()
    {
        targetLocalPosition = transform.localPosition;
    }

    private void Update()
    {
        HandleMovement();
        HandleTilt();
    }

    private void HandleMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 input = new Vector3(h, v, 0f).normalized;

        targetLocalPosition += input * moveSpeed * Time.deltaTime;

        targetLocalPosition.x = Mathf.Clamp(targetLocalPosition.x, rangeX.x, rangeX.y);
        targetLocalPosition.y = Mathf.Clamp(targetLocalPosition.y, rangeX.x, rangeX.y);
        targetLocalPosition.z = 0f;

        transform.localPosition = targetLocalPosition;
    }

    private void HandleTilt()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Quaternion targetRotation = Quaternion.Euler(-v * pitchAmount, h * yawAmount, -h * rollAmount);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, tiltSmooth * Time.deltaTime);
    }
}
