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

        // Variable para la dirección del movimiento
        float speed = moveSpeed * (Input.GetKey(KeyCode.LeftShift) ? sprintMultiplier : 1f);

        // Ajustamos el movimiento con respecto a la cámara
         // Ajustamos el movimiento con respecto a la cámara
        if (moveDirection.magnitude >= 0.1f)
        {
            // Convertimos la dirección de movimiento en función de la cámara
            Quaternion rotation = Quaternion.Euler(0, cameraYRotation, 0);
            Vector3 moveDir = rotation * moveDirection;

            // Mover el personaje
            controller.Move(moveDir * speed * Time.deltaTime);

            // Actualizar el Animator con la dirección en el sistema local
            animator.SetFloat("Speed", speed);
            animator.SetFloat("HorizontalDirection", (float)horizontal); 
            animator.SetFloat("VerticalDirection", (float)vertical); 
        } 
        else
        {
            animator.SetFloat("Speed", 0);
            animator.SetFloat("HorizontalDirection", 0);
            animator.SetFloat("VerticalDirection", 0);
        }

        // Aplicar gravedad y salto
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            velocity.y = Mathf.Sqrt(jumpForce * 2f * gravity);

        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        animator.SetFloat("SpeedY", 1);
    }



}
