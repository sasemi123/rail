using UnityEngine;
using UnityEngine.Pool;

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

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
