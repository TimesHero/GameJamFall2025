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
    [SerializeField] private float m_player_speed = 5;
    private Vector2 m_movement_direction;

    // Attributes: Control Schemes
    [SerializeField] private InputActionReference movement_controls;

    // === END: Attributes ===

    // -------------------------------------------------------------

    // === START: Unity Methods ===

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        player_body = player_object.GetComponent<Rigidbody2D>();
        player_sprite = player_object.GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
    }

    // === END: Unity Methods ===

    // -------------------------------------------------------------

    // === START: Custom Methods ===

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
    private void triggerIFrames(int iframe_duration_sec = 3, float iframe_opacity = 0.6f, int total_flashes = 1, int player_layer = 9, int enemy_layer = 10) {

        // Disable then Enable Collisions between layers
        // Player and Enemy layers must be separate
        Physics2D.IgnoreLayerCollision(player_layer, enemy_layer, true);

        // If we need N total flashes
        // We need iframe_duration_sec/N for each (to make them equal duration)
        float flash_duration = ((float) iframe_duration_sec)/((float) total_flashes);

        // We Make the model non-opaque red then white again to cause a flash
        for (int current_flash = 0; current_flash < total_flashes; i++) {

            m_player_sprite.color = new Color(1, 0, 0, iframe_opacity);
            yield return new WaitForSeconds(flash_duration);
            m_player_sprite = Color.white;

        }

        Physics2D.IgnoreLayerCollision(player_layer, enemy_layer, false);

    }


    // === END: Custom Methods ===

}
