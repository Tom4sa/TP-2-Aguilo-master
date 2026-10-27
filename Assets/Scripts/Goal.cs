using UnityEngine;

public class Goal : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>() != null && GameManager.Instance != null)
            GameManager.Instance.Ganar();
    }
}
