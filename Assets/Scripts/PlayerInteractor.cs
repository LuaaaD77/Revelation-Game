using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    public Camera playerCamera;
    public float interactDistance = 3.2f;
    public HUDController hud;

    void Update()
    {
        if (playerCamera == null) return;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward,
            out RaycastHit hit, interactDistance))
        {
            Interactable target = hit.collider.GetComponentInParent<Interactable>();
            if (target != null)
            {
                hud?.SetPrompt("E / INTERACT  •  " + target.Prompt);
                if (Input.GetKeyDown(KeyCode.E) || MobileInteractButton.Consume())
                    target.Interact();
                return;
            }
        }
        hud?.SetPrompt("");
        MobileInteractButton.Consume();
    }
}
