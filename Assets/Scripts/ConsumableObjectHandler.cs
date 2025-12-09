using UnityEngine;

public class ConsumableObjectHandler : MonoBehaviour
{
    float m_val = 1f;
    PlayerController m_player_controller;
    void Start()
    {
        // determine value and assign to the object
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // if val < tornado_val or if val <= tornado_val
        Destroy(gameObject);
        m_player_controller = collision.GetComponent<PlayerController>();
        m_player_controller.increaseTornadoValue(m_val / 10);
    }
}
