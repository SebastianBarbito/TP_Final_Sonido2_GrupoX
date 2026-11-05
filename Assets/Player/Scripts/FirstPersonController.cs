using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 5.0f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    [Header("Cámara y Mouse")]
    public Transform playerCamera;
    public float mouseSensitivity = 100.0f;
    public float maxLookAngle = 80.0f; // Límite para no dar la vuelta completa verticalmente

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float xRotation = 0.0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // Bloquear y ocultar el puntero del mouse en el centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // 1. Detección de suelo
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Mantener al jugador pegado al suelo
        }

        // 2. Rotación de la Cámara (Mouse Look)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxLookAngle, maxLookAngle); // Evita rotar de más hacia arriba/abajo

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // 3. Movimiento del Jugador (WASD / Flechas)
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        // 4. Salto
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // 5. Aplicar Gravedad
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}