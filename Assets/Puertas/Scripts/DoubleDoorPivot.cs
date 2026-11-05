using UnityEngine;

public class DoubleDoorPivot : MonoBehaviour
{
    [Header("Referencias de las Hojas")]
    public Transform leftDoor;
    public Transform rightDoor;

    [Header("Configuración de Apertura")]
    public float openAngle = 90.0f;
    public float openSpeed = 3.0f;
    public bool invertDirection = true; // Marca o desmarca esto en el Inspector si abre al revés

    private bool isOpen = false;
    private float currentAngle = 0.0f;
    private float targetAngle = 0.0f;

    private Vector3 leftHingePoint;
    private Vector3 rightHingePoint;

    void Start()
    {
        if (leftDoor != null)
        {
            // Posición del borde exterior (bisagra) de la puerta izquierda
            leftHingePoint = leftDoor.position - leftDoor.right * (leftDoor.GetComponent<Renderer>().bounds.size.x / 2f);
        }

        if (rightDoor != null)
        {
            // Posición del borde exterior (bisagra) de la puerta derecha
            rightHingePoint = rightDoor.position + rightDoor.right * (rightDoor.GetComponent<Renderer>().bounds.size.x / 2f);
        }
    }

    void Update()
    {
        float deltaAngle = Mathf.MoveTowards(currentAngle, targetAngle, openSpeed * 100f * Time.deltaTime) - currentAngle;
        currentAngle += deltaAngle;

        if (Mathf.Abs(deltaAngle) > 0.001f)
        {
            if (leftDoor != null)
                leftDoor.RotateAround(leftHingePoint, Vector3.up, deltaAngle);

            if (rightDoor != null)
                rightDoor.RotateAround(rightHingePoint, Vector3.up, -deltaAngle);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 doorToPlayer = other.transform.position - transform.position;
            float dot = Vector3.Dot(transform.forward, doorToPlayer);

            // Invertimos la dirección según el parámetro
            float direction = (dot >= 0 ? 1.0f : -1.0f) * (invertDirection ? -1.0f : 1.0f);

            targetAngle = openAngle * direction;
            isOpen = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetAngle = 0.0f;
            isOpen = false;
        }
    }
}