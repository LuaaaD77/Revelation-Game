using UnityEngine;

public class HeavenlyGateway : Interactable
{
    public Transform gateVisual;
    public override void Interact()
    {
        if (GameManager.Instance == null) return;
        if (!GameManager.Instance.GatewayUnlocked)
        {
            int remaining = GameManager.TotalFragments - GameManager.Instance.FragmentsFound;
            FindObjectOfType<HUDController>()?.ShowNotice("The gateway remains sealed. Find " + remaining + " more fragment(s).");
            return;
        }
        if (gateVisual != null) gateVisual.Rotate(Vector3.up, 90f);
        GameManager.Instance.CompleteChapter();
    }
}
