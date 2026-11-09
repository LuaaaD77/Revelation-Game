using UnityEngine;
using UnityEngine.EventSystems;

public class MobileInput : MonoBehaviour
{
    public static Vector2 Move { get; private set; }
    public static bool JumpPressed { get; private set; }
    static Vector2 lookDelta;
    public RectTransform joystickArea;
    public RectTransform joystickKnob;
    public RectTransform lookArea;

    int moveFinger = -1, lookFinger = -1;
    Vector2 moveOrigin, lastLook;
    float joystickRadius = 70f;

    void Update()
    {
        JumpPressed = false;
        if (Input.touchCount == 0) { Move = Vector2.zero; moveFinger = -1; lookFinger = -1; return; }
        foreach (Touch t in Input.touches)
        {
            if (t.phase == TouchPhase.Began)
            {
                if (t.position.x < Screen.width * 0.42f && moveFinger == -1)
                {
                    moveFinger = t.fingerId;
                    moveOrigin = t.position;
                }
                else if (lookFinger == -1) { lookFinger = t.fingerId; lastLook = t.position; }
            }
            if (t.fingerId == moveFinger)
            {
                Vector2 delta = (t.position - moveOrigin) / joystickRadius;
                Move = Vector2.ClampMagnitude(delta, 1f);
                if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
                { moveFinger = -1; Move = Vector2.zero; }
            }
            if (t.fingerId == lookFinger)
            {
                if (t.phase == TouchPhase.Moved) lookDelta += t.position - lastLook;
                lastLook = t.position;
                if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled) lookFinger = -1;
            }
        }
    }

    public static Vector2 ConsumeLook()
    {
        Vector2 result = lookDelta;
        lookDelta = Vector2.zero;
        return result;
    }

    public void TriggerJump() { JumpPressed = true; }
}
