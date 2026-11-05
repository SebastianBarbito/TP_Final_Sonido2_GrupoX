using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Configuración de Apertura")]
    public Vector3 openOffset = new Vector3(-1.5f, 0f, 0f); // Cuánto se desplaza al abrirse
    public float openSpeed = 3.0f; // Velocidad de movimiento

    [Header("Interacción")]
    public bool openOnApproach = true; // Si es true se abre solo al acercarse; si es false requiere presionar E

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isOpen = false;
    private bool playerIsNear = false;

    void Start()
    {
        closedPosition = transform.localPosition;
        openPosition = closedPosition + openOffset;
    }

    void Update()
    {
        // Interacción por tecla (E)
        if (!openOnApproach && playerIsNear && Input.GetKeyDown(KeyCode.E))
        {
            isOpen = !isOpen;
        }

        // Transición suave hacia la posición objetivo
        Vector3 targetPos = isOpen ? openPosition : closedPosition;
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, Time.deltaTime * openSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = true;
            if (openOnApproach) isOpen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = false;
            if (openOnApproach) isOpen = false;
        }
    }
}