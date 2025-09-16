using UnityEngine;

public class SandBag : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Sword"))
        {
            Debug.Log("Kepukul");
        }
    }
}
