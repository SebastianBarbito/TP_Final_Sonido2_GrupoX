using UnityEngine;

public class SingleDoorPivot : MonoBehaviour
{
    [Header("Referencia de la Hoja")]
    public Transform doorMesh; // Arrastra el objeto de la puerta aquí

    [Header("Configuración de Apertura")]
    public float openAngle = 90.0f;
    public float openSpeed = 3.0f;
    public bool invertDirection = false; // Cambia si abre hacia el lado contrario

    private float currentAngle = 0.0f;
    private float targetAngle = 0.0f;
    private Vector3 hingePoint;

    void Start()
    {
        if (doorMesh != null)
        {
            // Calcula el borde lateral (bisagra) automáticamente usando sus dimensiones
            hingePoint = doorMesh.position - doorMesh.right * (doorMesh.GetComponent<Renderer>().bounds.size.x / 2f);
        }
    }

    void Update()
    {
        if (doorMesh == null) return;

        float deltaAngle = Mathf.MoveTowards(currentAngle, targetAngle, openSpeed * 100f * Time.deltaTime) - currentAngle;
        currentAngle += deltaAngle;

        if (Mathf.Abs(deltaAngle) > 0.001f)
        {
            doorMesh.RotateAround(hingePoint, Vector3.up, deltaAngle);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 doorToPlayer = other.transform.position - transform.position;
            float dot = Vector3.Dot(transform.forward, doorToPlayer);

            float direction = (dot >= 0 ? 1.0f : -1.0f) * (invertDirection ? -1.0f : 1.0f);
            targetAngle = openAngle * direction;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetAngle = 0.0f;
        }
    }
}