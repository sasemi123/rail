using UnityEngine;
using UnityEngine.Pool;

public class ScriptPlayerProjectile : MonoBehaviour
{
    [Header("Features")]
    [SerializeField] private float speed = 120f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifeTime = 3f;

    [Header("Collision")]
    [SerializeField] private LayerMask hitMask;
    [SerializeField] private GameObject hitVfxPrefab;

    private IObjectPool<ScriptPlayerProjectile> pool;
    private float aliveTimer;

    public void SetPool(IObjectPool<ScriptPlayerProjectile> targetPool)
    {
        pool = targetPool;
    }

    public void Launch(Vector3 startPosition, Vector3 direction)
    {
        transform.SetPositionAndRotation(startPosition, Quaternion.LookRotation(direction));
        aliveTimer = 0f;
    }

    private void Update()
    {
        float step = speed * Time.deltaTime;

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, step, hitMask, QueryTriggerInteraction.Collide))
        {
            if (hit.collider.TryGetComponent<IDamageable>(out var target))
                target.TakeDamage(damage);

            if (hitVfxPrefab != null)
                Instantiate(hitVfxPrefab, hit.point, Quaternion.LookRotation(hit.normal));

            ReturnToPool();
            return;
        }

        transform.position += transform.forward * step;

        aliveTimer += Time.deltaTime;
        if (aliveTimer >= lifeTime)
            ReturnToPool();
    }

    private void ReturnToPool()
    {
        if (pool != null)
            pool.Release(this);
        else
            Destroy(gameObject);
    }

}
