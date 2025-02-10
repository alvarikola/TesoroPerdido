using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement")]
    public float moveSpeed = 5f;
    public float sprintMultiplier = 1.5f;
    public float jumpForce = 7f;
    public float gravity = 9.81f;
    
    [Header("References")]
    public CharacterController controller;
    public Transform cameraTransform;  // Referencia a la cámara para seguir la rotación

    private Vector3 velocity;
    private bool isGrounded;
    private Animator animator;

    void Start()
    {
        if (controller == null)
            controller = GetComponent<CharacterController>();
        
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        isGrounded = controller.isGrounded;

        // Obtener la rotación de la cámara solo en el eje Y
        float cameraYRotation = cameraTransform.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0, cameraYRotation, 0);  // Aplica la rotación al jugador

        // Obtener entrada de movimiento (WASD)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;

        // Actualizamos la velocidad de movimiento y la dirección
        float speed = moveSpeed * (Input.GetKey(KeyCode.LeftShift) ? sprintMultiplier : 1f);
        float movementDirection = 0f;  // Inicializamos la dirección del movimiento

        // Aplicar dirección de movimiento basada en la cámara
        if (moveDirection.magnitude >= 0.1f)
        {
            // Convertimos la dirección de movimiento en función de la cámara
            Quaternion rotation = Quaternion.Euler(0, cameraYRotation, 0);
            Vector3 moveDir = rotation * moveDirection;

            // Movimiento hacia adelante o atrás
            if (vertical > 0) movementDirection = 1f;  // Adelante
            else if (vertical < 0) movementDirection = -1f;  // Atrás
            // Movimiento lateral
            else if (horizontal > 0) movementDirection = 0.5f; // Derecha
            else if (horizontal < 0) movementDirection = -0.5f; // Izquierda

            // Aplicar movimiento
            controller.Move(moveDir * speed * Time.deltaTime);

            // Actualizamos el parámetro en el Animator
            animator.SetFloat("Speed", speed);
            animator.SetFloat("MovementDirection", movementDirection);
        } else {
            animator.SetFloat("Speed", 0);
        }

        // Aplicar gravedad y salto
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            velocity.y = Mathf.Sqrt(jumpForce * 2f * gravity);

        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }


}
