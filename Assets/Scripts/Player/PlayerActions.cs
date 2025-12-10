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



    // === END: Custom Methods ===

}
