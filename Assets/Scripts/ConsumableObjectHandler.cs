using UnityEngine;

public class ConsumableObjectHandler : MonoBehaviour
{
    [SerializeField] float m_val;
    PlayerController m_player_controller;
    // 1 = small_bush, 2 = big_bush, 3 = tree, 4 = house, 5 = big tree, 6 = car
    [SerializeField] int m_obstacle_type;
    void Start()
    {
    }
    // arbitrary comment for testing purposes
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>().getTornadoVal() >= m_val)
        {
            Destroy(gameObject);
            m_player_controller = collision.GetComponent<PlayerController>();
            m_player_controller.increaseTornadoValue(m_val / 10);
            m_player_controller.incrementConsumeStats(m_obstacle_type);
        }
    }
}
