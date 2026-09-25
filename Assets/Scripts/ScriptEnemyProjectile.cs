using UnityEngine;

public class ScriptEnemyProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 50f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifeTime = 6f;
    [SerializeField] private LayerMask hitMask;
    [SerializeField] private GameObject hitVfxPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;

        if (Physics.Raycast(transform.position, transform.forward,
                            out RaycastHit hit, step, hitMask, QueryTriggerInteraction.Collide))
        {
            if (hit.collider.TryGetComponent<IDamageable>(out var target))
                target.TakeDamage(damage);

            if (hitVfxPrefab != null)
                Instantiate(hitVfxPrefab, hit.point, Quaternion.LookRotation(hit.normal));

            Destroy(gameObject);
            return;
        }

        transform.position += transform.forward * step;
    }
}
