using UnityEngine;

public class DoorButton : MonoBehaviour
{
    public FNAFDoor door;
    public Renderer buttonRenderer;

    public Color redColor = Color.red;
    public Color greenColor = Color.green;

    private Material buttonMat;

    void Start()
    {
        buttonMat = buttonRenderer.material;
        UpdateButtonColor();
    }

    void OnMouseDown()
    {
        if (door == null) return;

        door.ToggleDoor();
        UpdateButtonColor();
    }

    void UpdateButtonColor()
    {
        // Green = Open (door up)
        // Red   = Closed (door down)
        buttonMat.color = door.IsOpen() ? redColor : greenColor;
    }
}