using UnityEngine;

public class Door : MonoBehaviour
{
    public Animation animations;

    private bool isOpen = false;

    private void Start()
    {
        animations = GetComponent<Animation>();
    }

    public void ToggleDoor()
    {
        if (isOpen)
        {
            animations.Play("CloseDoorAnimation");
            Debug.Log("tozita");
        }
        else
        {
            animations.Play("OpenDoorAnimation");
            Debug.Log("hiraita");
        }
        isOpen = !isOpen;
    }
}
