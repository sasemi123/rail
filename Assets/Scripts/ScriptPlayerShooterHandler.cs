using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;
using UnityEngine.WSA;

[RequireComponent(typeof(AudioSource))]
public class ScriptPlayerShooterHandler : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private ScriptCrosshairHandler crosshairHandlerScript;
    [SerializeField] private Transform[] launchers;
    [SerializeField] private ScriptPlayerProjectile projectilePrefab;

    [Header("RateOfFire")]
    [SerializeField] private float fireRate = 8f;

    [Header("Pool")]
    [SerializeField] private int defaultPoolSize = 30;
    [SerializeField] private int maxPoolSize = 100;

    [Header("Effect")]
    [SerializeField] private ParticleSystem[] flashes;
    [SerializeField] private AudioClip fireSfx;
    [Range(0f, 1f)][SerializeField] private float fireSfxVolume = 0.5f;

    private IObjectPool<ScriptPlayerProjectile> pool;
    private AudioSource audioSource;
    private float nextFireTime;
    private int launcherIndex;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;

        pool = new ObjectPool<ScriptPlayerProjectile>(
            createFunc: CreateProjectile, 
            actionOnGet: OnGetProjectile,
            actionOnRelease: OnReleaseProjectile, 
            actionOnDestroy: OnDestroyProjectile, 
            collectionCheck: false,
            defaultCapacity: defaultPoolSize, 
            maxSize: maxPoolSize);
    }

    private ScriptPlayerProjectile CreateProjectile()
    {
        ScriptPlayerProjectile instance = Instantiate(projectilePrefab);
        instance.SetPool(pool);
        return instance;
    }

    private void OnGetProjectile(ScriptPlayerProjectile p) => p.gameObject.SetActive(true);

    private void OnReleaseProjectile(ScriptPlayerProjectile p) => p.gameObject.SetActive(false);

    private void OnDestroyProjectile(ScriptPlayerProjectile p) => Destroy(p.gameObject);

    private void Update()
    {
        /*
        if (ScriptGameManager.Instance != null &&
            ScriptGameManager.Instance.State != ScriptGameManager.GameState.Playing)
            return;
        */

        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + (1f / fireRate);
        }
    }

    private void Fire()
    {
        if (launchers == null || launchers.Length == 0 || crosshairHandlerScript == null) return;

        Transform launcher = launchers[launcherIndex];
        launcherIndex = (launcherIndex + 1) % launchers.Length;

        Vector3 direction = (crosshairHandlerScript.AimPoint - launcher.position).normalized;

        ScriptPlayerProjectile shot = pool.Get();
        shot.Launch(launcher.position, direction);

        PlayFireEffects();
    }

    private void PlayFireEffects()
    {
        if (flashes != null)
        {
            foreach (var flash in flashes)
                if (flash != null) flash.Play();
        }

        if (fireSfx != null)
            audioSource.PlayOneShot(fireSfx, fireSfxVolume);
    }
}
