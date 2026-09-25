using UnityEngine;

public class ScriptEnemyShooterHandler : MonoBehaviour
{
    [Header("Refernce")]
    [SerializeField] private Transform launcher;
    [SerializeField] private GameObject enemyProjectilePrefab;

    [Header("Shooting")]
    [SerializeField] private float fireInterval = 1.6f;
    [SerializeField] private float inaccuracy = 1.8f;
    [SerializeField] private float initialDelay = 1f;

    [Header("Effect")]
    [SerializeField] private ParticleSystem flash;
    [SerializeField] private AudioClip fireSfx;

    private float timer;
    private bool hasStarted;

    public void TryShoot(Transform target)
    {
        if (target == null || enemyProjectilePrefab == null || launcher == null) return;

        timer += Time.deltaTime;

        float threshold = hasStarted ? fireInterval : initialDelay;
        if (timer < threshold) return;

        timer = 0f;
        hasStarted = true;

        Shoot(target);
    }

    private void Shoot(Transform target)
    {
        // Add some inaccuracy so players can dodge and the game isn't too brutal.
        Vector3 aimPoint = target.position + Random.insideUnitSphere * inaccuracy;
        Vector3 direction = (aimPoint - launcher.position).normalized;

        Instantiate(enemyProjectilePrefab, launcher.position, Quaternion.LookRotation(direction));

        if (flash != null) flash.Play();
        if (fireSfx != null) AudioSource.PlayClipAtPoint(fireSfx, launcher.position, 0.4f);
    }
}
