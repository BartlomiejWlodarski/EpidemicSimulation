using UnityEngine;

public class MouseCamera : MonoBehaviour
{
    public bool active = false;
    public Vector2 turn;
    public float sensitivity = .5f;
    public Vector3 deltaMove;
    public float speed = 1;
    public GameObject mover;

    [Header("Camera bounds")]
    public float xMax = 20;
    public float xMin = -20;
    public float yMax = 25;
    public float yMin = 0.5f;
    public float zMax = 35;
    public float zMin = -20;

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        if (active)
        {
            if (Input.GetKey(KeyCode.Mouse2))
            {
                turn.x += Input.GetAxis("Mouse X") * sensitivity;
                turn.y += Input.GetAxis("Mouse Y") * sensitivity;
                transform.localRotation = Quaternion.Euler(-turn.y, turn.x, 0);
            }

            deltaMove = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")) * speed * Time.deltaTime;
            transform.Translate(deltaMove);

            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, xMin, xMax),
                Mathf.Clamp(transform.position.y, yMin, yMax),
                Mathf.Clamp(transform.position.z, zMin, zMax)
                );
        }
    }
}
