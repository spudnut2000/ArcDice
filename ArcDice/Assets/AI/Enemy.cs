using UnityEngine.AI;
using UnityEngine;

public class Enemy : PoolableObject
{
    public NavMeshAgent Agent;
    public EnemyScriptableObject EnemyScriptableObject;
    public float Health = 100;
    public EnemyMovement enemyMovement;
    public Animator Animator;
    public Collider collision;
    public Rigidbody rb;
    public bool isBoss;
    private GameObject Player;
    public GameManager gameManager;
    private AudioManager audioManager;

    public virtual void OnEnable()
    {
        SetUpAgentFromConfiguration();
        Agent.enabled = true;
        Animator = GetComponent<Animator>();
        enemyMovement = GetComponent<EnemyMovement>();
        collision = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        Player = enemyMovement.Player.gameObject;
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        Debug.Log(gameManager);
    }

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();

        if (isBoss)
        {
            audioManager.PlayAudio(audioManager.bossMusic);
        }
        
    }

    public override void OnDisable()
    {
        base.OnDisable();

        Agent.enabled = false;
    }

    public virtual void SetUpAgentFromConfiguration()
    {
        Agent.acceleration = EnemyScriptableObject.Acceleration;
        Agent.angularSpeed = EnemyScriptableObject.AngularSpeed;  
        Agent.areaMask = EnemyScriptableObject.AreaMask;
        Agent.avoidancePriority = EnemyScriptableObject.AvoidancePriority;
        Agent.baseOffset = EnemyScriptableObject.BaseOffset;
        Agent.height = EnemyScriptableObject.Height;
        Agent.obstacleAvoidanceType = EnemyScriptableObject.ObstacleAvoidanceType;
        Agent.radius = EnemyScriptableObject.radius;
        Agent.speed = EnemyScriptableObject.Speed;
        Agent.stoppingDistance = EnemyScriptableObject.StoppingDistance;
        isBoss = EnemyScriptableObject.Boss;

        
        Health = EnemyScriptableObject.Health;
        if (isBoss)
            Health = Health * gameManager.currentPlayerLevel;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
            Animator.SetTrigger("Attack");
    }

    private void Update()
    {
        if(Health <= 0)
        {
            //TODO Die
            Debug.Log(Health);
            OnDeath();
        }
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
    }

    private void OnDeath()
    {
        if (isBoss)
        {
            //Debug.Log("Here");
            Animator.SetTrigger("Death");
            rb.freezeRotation = true;
            gameManager.HatchOn();
            Destroy(gameObject, 4.0f);
            this.enabled = false;
            
            //Player.GetComponent<PlayerManager>().playerLevel++;
            //Debug.Log(GetComponent<PlayerManager>().playerLevel);
            return;
            
        }
            
        Animator.SetTrigger("Death");
        rb.freezeRotation = true;
        Destroy(gameObject, 4.0f);
        this.enabled = false;
    }
}
