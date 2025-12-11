using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MinimalPlayerActions : MonoBehaviour
{

    [SerializeField] private Camera m_main_camera;

    // Attributes: Player Entity
    [SerializeField] private GameObject m_player_object;
    private Rigidbody2D m_player_body;
    private Collider m_player_collider;
    private SpriteRenderer m_player_sprite;

    // Attributes: Movement Settings and variables
    // The max speed of the player IGNORING DIRECTION!
    [SerializeField] private float m_max_speed = 10000f;
    private float m_current_speed = 0f;
    [SerializeField] private float m_acceleration = 0.5f;
    private Vector2 m_current_velocity;
    // Direction (Uses m_current_direction)
    private Vector2 m_max_velocity;
    // Evaluating mouse position for direction gives a Vector3 to be handled
    private Vector2 m_current_direction = new Vector2(0, 0);

    // Attributes: Inputs
    [SerializeField] private InputActionReference m_move_confirm;

    void OnEnable() {
        m_move_confirm.action.started += triggerMove;
    }

    void OnDisable() {
        m_move_confirm.action.started -= triggerMove;
    }

    void Start() {

        m_player_body = m_player_object.GetComponent<Rigidbody2D>();
        m_player_collider = m_player_object.GetComponent<Collider>();
        m_player_sprite = m_player_object.GetComponent<SpriteRenderer>();

    }

    void Update() {

        // transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        // updateVelocity();
        // transform.position = Vector3.MoveTowards(transform.position, m_current_direction, m_max_speed * Time.deltaTime);

        updateVelocity();
        m_player_body.linearVelocity = m_current_velocity;
        // Debug.Log("Current Direction: " + VectorMath.printVector3(m_current_direction));
        // Debug.Log("Current Position: " + VectorMath.printVector3(transform.position));
        // Debug.Log("Current Max Velocity: " + VectorMath.printVector3(m_max_velocity));

    }

    private void triggerMove(InputAction.CallbackContext obj) {

        Debug.Log("recieved click");

        updateCurrentDirection();

        updateMaxVelocity();


    }

    private void updateCurrentDirection() {

        Vector3 current_mouse_pos = m_main_camera.ScreenToWorldPoint(VectorMath.getMousePos());

        m_current_direction = new Vector2(current_mouse_pos.x - transform.position.x, current_mouse_pos.y - transform.position.y);
        m_current_direction.Normalize();
        // m_current_direction = m_current_direction.Normalize();


        Debug.Log("Current Direction on Click: " + VectorMath.printVector2(m_current_direction));

    }

    private void updateVelocity() {

        float delta_x = m_current_direction.x * m_acceleration;
        float delta_y = m_current_direction.y * m_acceleration;

        Debug.Log("Delta X: " + delta_x.ToString());
        Debug.Log("Delta Y: " + delta_y.ToString());

        m_current_velocity.x += delta_x;
        m_current_velocity.y += delta_y;

        Debug.Log("Velocity X: " + m_current_velocity.x.ToString());
        Debug.Log("Velocity Y: " + m_current_velocity.y.ToString());

        if (delta_x < 0) {
            if (m_current_velocity.x < m_max_velocity.x) { m_current_velocity.x = m_max_velocity.x; }
        }
        else if (delta_x > 0) {

            if (m_current_velocity.x > m_max_velocity.x) { m_current_velocity.x = m_max_velocity.x; }
        }

        if (delta_y < 0) {
            if (m_current_velocity.y < m_max_velocity.y) { m_current_velocity.y = m_max_velocity.y; }
        }
        else if (delta_y > 0) {

            if (m_current_velocity.y > m_max_velocity.y) { m_current_velocity.y = m_max_velocity.y; }
        }

        Debug.Log("Max Velocity X: " + m_max_velocity.x.ToString());
        Debug.Log("Max Velocity Y: " + m_max_velocity.y.ToString());
        Debug.Log("Limited Velocity X: " + m_current_velocity.x.ToString());
        Debug.Log("Limited Velocity Y: " + m_current_velocity.y.ToString());
        // Debug.Log("Current Velocity: " + VectorMath.printVector3(m_current_velocity));

    }

    private void updateMaxVelocity() {

        m_max_velocity.x = m_current_direction.x * m_max_speed;
        m_max_velocity.y = m_current_direction.y * m_max_speed;

    }


}
