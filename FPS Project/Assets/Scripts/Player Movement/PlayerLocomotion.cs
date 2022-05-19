using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    Animator animator;
    Vector2 input;
    [SerializeField] float lerpSpeed = 5f;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        input = Vector2.Lerp(input, playerMovement.controls.Player.Movement.ReadValue<Vector2>(), lerpSpeed * Time.deltaTime);

        animator.SetFloat("InputX", input.x);
        animator.SetFloat("InputY", input.y);

        if (playerMovement.sprinting && input != Vector2.zero)
        {
            animator.SetFloat("Speed", 1.3f);
        }
        else
        {
            animator.SetFloat("Speed", 1f);
        }
    }
}
