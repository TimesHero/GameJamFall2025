/*
 * Project: Game Jam Fall 2025
 * File:    PlayerActions
 * Description: Sets up player behaviour and controls (+ behaviour methods)
 *
 * Author:  Lirael "El" Khan
 * Student Number: 301511913
 * Created: 2025-12-09
 *
 * Index:
 *  - Attributes
 *      - player entity
 *      - movement
 *      - input references
 *  - Unity Methods
 *      - Start: Set up player object variables
 *  - Custom Methods
 *
 *
 */

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{

    // === START: Attributes ===

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

    // === END: Attributes ===

    // -------------------------------------------------------------

    // === START: Unity Methods ===

    void OnEnable() {

        m_move_confirm.action.started += triggerPlayerMove;

    }

    void OnDisable() {

        m_move_confirm.action.started -= triggerPlayerMove;

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

        updatePlayerVelocity();

    }

    // === END: Unity Methods ===

    // -------------------------------------------------------------

    // === START: Custom Methods ===

    // === START: Getter/Setter Methods ===

    // === END: Getter/Setter Methods ===

    /*
    * @Brief: Gets a vector of the max player velocity
    *
    * - Uses build in Unity attribute to get mouse position
    * - The final vector must reflect the speed in both x and y directions
    *
    * @Return: (Vector3) Max velocity vector (max velocity in each direction)
    */
    // private Vector3 getMaxPlayerVelocity() {
    //
    //     // Unity built in way to get vector for mouse position
    //     Vector3 current_mouse_pos = Input.mousePosition;
    //
    //     // The players max speed IGNORES DIRECTION
    //     // Need to scalar multiply to get directional velocity
    //     Vector3 max_velocity = new Vector3(current_mouse_pos.x * m_max_player_speed,
    //                                             current_mouse_pos.y * m_max_player_speed, 0);
    //
    //         return max_velocity;
    // }

    /*
    * @Brief: Uses the current mouse position to set the new player direction
    *
    * @NOTE: Only called when the player clicks to change/update direction
    * @NOTE: Modifies movement direction
    *
    * @Return: N/A
    */
    private void updateDirection() {

        m_movement_direction = Vector3.Normalize(VectorMath.getMouseCoord());
        Debug.Log("New direction vec3: " + VectorMath.printVector3(m_movement_direction));


    }

    /*
    * @Brief: The maximum velocity vector for player movement currently
    *
    * @NOTE: Only called when the player clicks to change/update direction
    * @NOTE: Modifies movement direction
    * @NOTE: Modifies max velocity
    *
    * @Return: N/A
    */
    private void updatePlayerMaxVelocity() {

        m_current_max_velocity = VectorMath.vector3ScalarMultiply(m_movement_direction, m_max_player_speed);

        Debug.Log("New direction vec3: " + VectorMath.printVector3(m_current_max_velocity));
    }

    /*
    * @Brief: Updates the velocity (current speed and direction) of the player
    *
    * - Uses mouse position to set direction
    * - Sets velocity
    * - ramps velocity until max player speed reached reached or direction changed
    *
    * @Return: N/A
    */
    private void updatePlayerVelocity() {

        float current_velocity_x = VectorMath.multFloatWithLimit(
                                    m_movement_direction.x,
                                    m_player_acceleration,
                                    m_current_max_velocity.x
                );
        float current_velocity_y = VectorMath.multFloatWithLimit(
                                    m_movement_direction.y,
                                    m_player_acceleration,
                                    m_current_max_velocity.y
                );


        m_current_velocity = new Vector2(current_velocity_x,
                                        current_velocity_y
                                        );

            m_player_body.linearVelocity = m_current_velocity;


    }

    private void triggerPlayerMove(InputAction.CallbackContext obj) {

        updateDirection();

        updatePlayerMaxVelocity();

    }

    /*
    * @Brief: Activates iframes for specified duration then deactivates
    *
    * - Activates iframes (disable collisions with enemies)
    * - Flash red desired times
    * - Deactivate iframes
    * - Uses player and enemy layers to toggle collisions on/off
    *
    * @Arg: iframe_duration_sec => Duration to keep iframes active (IN SECONDS)
    * @Arg: iframe_opacity => The player sprite opacity while iframes are active
    * @Arg: total_flashes => Times player sprite flashes during iframes
    * @Arg: player_layer => Layer used for player
    * @Arg: enemy_layer => Layer used for enemies
    *
    * @WARNING: Enemies and Player MUST be on different layers
    * @WARNING: USES yeild -> WaitForSeconds (Requires subroutine)
    *
    * @Return: N/A
    */
    private IEnumerator triggerIFrames(int iframe_duration_sec = 3,
                                        float iframe_opacity = 0.6f, int total_flashes = 1,
                                        int player_layer = 9, int enemy_layer = 10) {

        // Disable then Enable Collisions between layers
        // Player and Enemy layers must be separate
        Physics2D.IgnoreLayerCollision(player_layer, enemy_layer, true);

        // If we need N total flashes
        // We need iframe_duration_sec/N for each (to make them equal duration)
        float flash_duration = ((float) iframe_duration_sec)/((float) total_flashes);
        // We want it to flash red then normal EACH for half the duration of a flash
        float half_flash_duration = flash_duration/2;

        // We Make the model non-opaque red then white again to cause a flash
        for (int current_flash = 0; current_flash < total_flashes; current_flash++) {

            // Flash Mechansism
            // (1) Add sprite red tint
            // (2) wait half flash
            // (3)turn sprite normal color
            // (4) wait half flash
            m_player_sprite.color = new Color(1, 0, 0, iframe_opacity);
            yield return new WaitForSeconds(half_flash_duration);
            m_player_sprite.color = Color.white;
            yield return new WaitForSeconds(half_flash_duration);

        }

        Physics2D.IgnoreLayerCollision(player_layer, enemy_layer, false);

    }

    public void triggerDamage() {
        // triggerIFrames(m_iframe_duration_sec, 0.8f, 3);
    }

    // === END: Custom Methods ===

}
