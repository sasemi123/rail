using UnityEngine;

public class ScriptCrosshairHandler : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Camera cam;
    [SerializeField] private RectTransform crosshair;

    [Header("Aiming")]
    [SerializeField] private LayerMask aimMask;
    [SerializeField] private float maxAimDistance = 400f;

    public Vector3 AimPoint { get; private set; }

    public Transform AimTarget { get; private set; }

    private void Awake()
    {
        if (cam == null) cam = Camera.main;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        Vector3 screenPosition = Input.mousePosition;

        if (crosshair != null)
            crosshair.position = screenPosition;

        Ray ray = cam.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit, maxAimDistance, aimMask, QueryTriggerInteraction.Ignore))
        {
            AimPoint = hit.point;
            AimTarget = hit.transform;
        }
        else
        {
            AimPoint = ray.GetPoint(maxAimDistance);
            AimTarget = null;
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(AimPoint, 0.5f);
    }
}
