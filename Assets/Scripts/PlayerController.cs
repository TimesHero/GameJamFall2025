using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Vector3 m_target_pos;
    float m_speed = 5f;
    float m_tornado_val = 1f;
    Vector3 m_scale_vector;
    [SerializeField] ScoreManager m_score_manager;
    // index 0 = total consumed, 1 = obstacle1
    int[] m_obstacles_consumed;
    void Start()
    {
        m_scale_vector = transform.localScale;
    }

    // temp name since I just wanted to have the functionality implemented, probably better to decouple from attack and
    // have the name be more normal later
    public void OnAttack()
    {
        m_target_pos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        m_target_pos.z = transform.position.z;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, m_target_pos, m_speed * Time.deltaTime);
        transform.localScale = m_scale_vector * m_tornado_val;
    }

    public void increaseTornadoValue(float val)
    {
        m_tornado_val += val;
    }

    public void incrementConsumeStats(int type)
    {
        m_obstacles_consumed[0]++;
        m_obstacles_consumed[type]++;
    }

    void OnDestroy()
    {
        m_score_manager.setConsumeStats(m_obstacles_consumed);
    }
}
