using UnityEngine;
using UnityEngine.Events;

public class ScriptHealthHandler : MonoBehaviour
{
    [Header("HP")]
    [SerializeField] private int maxHealth = 3;

    [Header("Score")]
    [SerializeField] private int scoreValue = 100;

    [Header("Effect")]
    [SerializeField] private GameObject deathVfxPrefab;
    [SerializeField] private AudioClip hitSfx;
    [SerializeField] private AudioClip deathSfx;

    [Header("Death Behaviour")]
    [SerializeField] private bool deactivateOnDeath = true;

    [Header("Event")]
    public UnityEvent<int, int> OnHealthChanged;
    public UnityEvent OnDamaged;
    public UnityEvent OnDied;

    private int currentHealth;
    private bool isDead;

    public int Current => currentHealth;
    public int Max => maxHealth;
    public bool IsDead => isDead;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        // Initialize the UI in Start, not Awake
        // to ensure the UI has been created and events have been wired up.
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        if (isDead || amount <= 0) return;

        currentHealth = Mathf.Max(0, currentHealth - amount);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnDamaged?.Invoke();

        if (hitSfx != null)
            AudioSource.PlayClipAtPoint(hitSfx, transform.position, 0.6f);

        if (currentHealth <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        if (isDead || amount <= 0) return;

        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void Die()
    {
        isDead = true;

        if (deathVfxPrefab != null)
            Instantiate(deathVfxPrefab, transform.position, Quaternion.identity);

        if (deathSfx != null)
            AudioSource.PlayClipAtPoint(deathSfx, transform.position, 0.8f);

        /*
        if (scoreValue > 0 && GameManager.Instance != null)
            ScriptGameManager.Instance.AddScore(scoreValue);
        */

        OnDied?.Invoke();

        if (deactivateOnDeath)
            gameObject.SetActive(false);
    }
}
