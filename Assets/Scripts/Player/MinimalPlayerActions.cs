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
    [SerializeField] private float m_max_speed = 5f;
    [SerializeField] private float m_acceleration = 1f;
    private Vector2 m_current_velocity;
    // Direction (Uses m_currrent_direction)
    private Vector2 m_max_velocity;
    // Evaluating mouse position for direction gives a Vector3 to be handled
    private Vector3 m_current_direction = new Vector3(0, 0, 0);

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
        updateVelocity();

    }

    private void triggerMove(InputAction.CallbackContext obj) {

        Debug.Log("recieved click");

        updateCurrentDirection();

        updateMaxVelocity();
    }

    private void updateCurrentDirection() {

        m_current_direction = m_main_camera.ScreenToWorldPoint(VectorMath.getMousePos());

        Vector3.Normalize(m_current_direction);

        m_current_direction.z = transform.position.z;


    }

    private void updateVelocity() {

        m_current_velocity.x = VectorMath.addFloatWithLimit(
                                            m_current_direction.x,
                                            m_current_direction.x * m_acceleration,
                                            m_max_velocity.x
                );

        m_current_velocity.y = VectorMath.addFloatWithLimit(
                                            m_current_direction.y,
                                            m_current_direction.y * m_acceleration,
                                            m_max_velocity.y
                );

        m_player_body.linearVelocity = m_current_velocity;

    }

    private void updateMaxVelocity() {

        m_max_velocity.x = m_current_direction.x * m_max_speed;
        m_max_velocity.y = m_current_direction.y * m_max_speed;

    }


}
