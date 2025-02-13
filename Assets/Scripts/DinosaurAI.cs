using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class DinosaurAI : MonoBehaviour
{
    public float roamRadius = 20f; // Radio de movimiento libre
    public float waitTime = 3f; // Tiempo de espera entre movimientos
    
    private NavMeshAgent agent;
    private Vector3 destination;
    private bool isWaiting = false;
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        MoveToNewPosition();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && !isWaiting)
        {
            StartCoroutine(WaitAndMove());
        }

        // Actualizar la velocidad en el Animator
        UpdateAnimatorSpeed();
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
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
        randomDirection += transform.position;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomDirection, out hit, roamRadius, NavMesh.AllAreas))
        {
            destination = hit.position;
            agent.SetDestination(destination);
            Debug.Log("Nuevo destino: " + destination);
        }
        else
        {
            Debug.Log("No se encontró una posición válida en el NavMesh.");
        }
    }

    void UpdateAnimatorSpeed()
    {
        // Si el agente está en movimiento, establecer la velocidad en el Animator
        if (agent.velocity.magnitude > 0.1f)
        {
            animator.SetFloat("speed", 1f);  // Ajusta el valor según la velocidad deseada
            //Debug.Log("Velocidad del agente: " + agent.velocity.magnitude);
        }
        else
        {
            animator.SetFloat("speed", 0f);  // Cuando no se mueve
        }
    }
}
