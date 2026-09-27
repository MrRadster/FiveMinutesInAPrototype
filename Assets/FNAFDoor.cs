using UnityEngine;

public class FNAFDoor : MonoBehaviour
{
    [Header("Door Settings")]
    public float openSpeed = 4f;
    public float openHeight = 2.6f;

    private Vector3 closedPos;
    private Vector3 openPos;
    private bool isOpen = true;        // Starts open
    private bool isMoving = false;

    void Start()
    {
        // Current position in the scene should be the OPEN position (door up)
        openPos = transform.position;

        // Closed position is lower (door drops down)
        closedPos = openPos - Vector3.up * openHeight;
    }

    void Update()
    {
        if (!isMoving) return;

        Vector3 target = isOpen ? openPos : closedPos;
        transform.position = Vector3.MoveTowards(transform.position, target, openSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            isMoving = false;
        }
    }

    public void ToggleDoor()
    {
        if (isMoving) return;

        isOpen = !isOpen;
        isMoving = true;
    }

    public bool IsOpen()
    {
        return isOpen;
    }
}