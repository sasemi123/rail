using UnityEngine;

public class ScriptEnemyHandler : MonoBehaviour
{
    public enum State { Enter, Attack, Leave }

    [Header("Reference")]
    [SerializeField] private ScriptEnemyShooterHandler enemyShooterHandlerScript;

    [Header("Entry")]
    [SerializeField] private Vector3 spawnOffset = new Vector3(0f, 0f, 50f);
    [SerializeField] private float enterSpeed = 25f;

    [Header("Attack")]
    [SerializeField] private float attackDuration = 3f;
    [SerializeField] private float bobAmptitude = 1.2f;
    [SerializeField] private float bobFrequency = 1.5f;
    [SerializeField] private float turnSpeed = 0f;

    [Header("Leave")]
    [SerializeField] private Vector3 leaveDirection = new Vector3(0f, 0f, -1f);
    [SerializeField] private float leaveSpeed = 25f;
    [SerializeField] private float leaveDespawnTime = 3f;

    private Transform player;
    private Vector3 attackPosition;
    private State state = State.Enter;
    private float stateTimer;
    private float bobSeed;

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null) player = playerObject.transform;

        attackPosition = transform.position;

        transform.position = attackPosition + spawnOffset;

        bobSeed = Random.Range(0f, 10f);
    }

    void Update()
    {
        stateTimer += Time.deltaTime;

        FacePlayer();

        switch (state)
        {
            case State.Enter: UpdateEnter(); break;
            case State.Attack: UpdateAttack(); break;
            case State.Leave: UpdateLeave(); break;
        }
    }

    private void FacePlayer()
    {
        if (player == null) return;

        Vector3 toPlayer = player.position - transform.position;
        if (toPlayer.sqrMagnitude < 0.01f) return;

        Quaternion look = Quaternion.LookRotation(toPlayer.normalized, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, look, turnSpeed * Time.deltaTime);
    }

    private void UpdateEnter()
    {
        transform.position = Vector3.MoveTowards(
            transform.position, attackPosition, enterSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, attackPosition) < 0.1f)
            ChangeState(State.Attack);
    }

    private void UpdateAttack()
    {
        float bobOffset = Mathf.Sin((Time.time + bobSeed) * bobFrequency) * bobAmptitude;
        transform.position = attackPosition + Vector3.up * bobOffset;

        if (enemyShooterHandlerScript != null && player != null)
            enemyShooterHandlerScript.TryShoot(player);

        if (stateTimer >= attackDuration)
            ChangeState(State.Leave);
    }

    private void UpdateLeave()
    {
        transform.position += leaveDirection.normalized * leaveSpeed * Time.deltaTime;

        if (stateTimer >= leaveDespawnTime)
            Destroy(gameObject);
    }

    private void ChangeState(State next)
    {
        state = next;
        stateTimer = 0f;
    }
}
