using UnityEngine;

public class MessageCollider : MonoBehaviour
{
    [SerializeField] GameObject Message;

    void OnCollisionStay2D(Collision2D collision)
    {
        Message.SetActive(true);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        Message.SetActive(false);
    }
}
