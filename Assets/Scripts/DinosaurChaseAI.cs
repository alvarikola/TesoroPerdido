using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement; // Necesario para reiniciar la escena

public class DinosaurChaseAI : MonoBehaviour
{
    public Transform player; // Referencia al jugador
    private float detectionRadius = 25f; // Distancia para detectar al jugador
    public float chaseSpeed = 5f; // Velocidad al perseguir
    public float roamSpeed = 2f; // Velocidad normal
    public float roamRadius = 20f; // Radio de patrulla
    public float waitTime = 3f; // Tiempo de espera entre movimientos

    private NavMeshAgent agent;
    private Vector3 destination;
    private bool isWaiting = false;
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        MoveToNewPosition();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            ChasePlayer();
        }
        else if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && !isWaiting)
        {
            StartCoroutine(WaitAndMove());
        }

        UpdateAnimatorSpeed();
    }

    void ChasePlayer()
    {
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);
    }

    IEnumerator WaitAndMove()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        MoveToNewPosition();
        isWaiting = false;
    }

    void MoveToNewPosition()
    {
        agent.speed = roamSpeed;
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, roamRadius, NavMesh.AllAreas))
        {
            destination = hit.position;
            agent.SetDestination(destination);
        }
    }

    void UpdateAnimatorSpeed()
    {
        float speedValue = agent.velocity.magnitude;
        animator.SetFloat("speed", speedValue);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Asegúrate de que el jugador tiene el tag "Player"
        {
            Debug.Log("¡El dinosaurio atrapó al jugador!");
            KillPlayer();
        }
    }

    void KillPlayer()
    {
        // Opción 1: Reiniciar la escena al morir
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // Opción 2: Desactivar al jugador en lugar de reiniciar la escena
        player.gameObject.SetActive(false);

        // Opción 3: Activar una pantalla de "Game Over"
        // gameOverScreen.SetActive(true);
    }
}
