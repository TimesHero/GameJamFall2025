using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MinimalPlayerActions : MonoBehaviour
{

    // Attributes: Player Entity
    [SerializeField] private GameObject m_player_object;
    private Rigidbody2D m_player_body;
    private SpriteRenderer m_player_sprite;

    // Attributes: Movement Settings and variables
    // The max speed of the player IGNORING DIRECTION!
    [SerializeField] private float m_max_player_speed = 5f;
    // Current and Max Velocities account for direction
    // These are tracked to manage player acceleration and movement changes
    private Vector3 m_current_max_velocity = new Vector3(0f, 0f, 0f);
    private Vector2 m_current_velocity = new Vector2(0f, 0f);
    [SerializeField] private float m_player_acceleration = 1f;
    private Vector3 m_movement_direction = new Vector3(0f, 0f, 0f);

    // Attributes: Control Schemes
    [SerializeField] private InputActionReference m_move_confirm;

    // Attributes: Damage Management
    [SerializeField] private int m_iframe_duration_sec = 3;

    void OnEnable() {
        m_move_confirm.action.started += triggerMove;
    }

    void OnDisable() {
        m_move_confirm.action.started -= triggerMove;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Setup player components using the player game object
        m_player_body = m_player_object.GetComponent<Rigidbody2D>();
        m_player_sprite = m_player_object.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        updateCurrentVelocity();
    }

    void triggerMove(InputAction.CallbackContext obj) {

        Vector3 mouse_pos = Input.mousePosition;
        Debug.Log(VectorMath.printVector3(mouse_pos));

        m_movement_direction = Vector3.Normalize(mouse_pos);
        Debug.Log(VectorMath.printVector3(m_movement_direction));

        m_current_max_velocity = new Vector3(m_movement_direction.x * m_max_player_speed,
                                m_movement_direction.y * m_max_player_speed,
                                m_movement_direction.z * m_max_player_speed);

        Debug.Log(VectorMath.printVector3(m_current_max_velocity));

    }
    
    void updateCurrentVelocity() {
        float new_velocity_x = m_current_velocity.x + (m_current_max_velocity * m_player_acceleration);
        float new_velocity_y = m_current_velocity.y + (m_current_max_velocity * m_player_acceleration);

        if (new_velocity_x > m_current_max_velocity.x) {
            new_velocity_x = m_current_max_velocity.x;
        }

        if (new_velocity_y > m_current_max_velocity.y) {
            new_velocity_y = m_current_max_velocity.y;
        }

        m_current_velocity = new Vector3(new_velocity_x, new_velocity_y);
        m_player_body.linearVelocity = m_current_velocity;

    }

}
