using UnityEngine;

public class MobileInteractButton : MonoBehaviour
{
    static bool pressed;
    public void Press() { pressed = true; }
    public static bool Consume() { bool value = pressed; pressed = false; return value; }
}
