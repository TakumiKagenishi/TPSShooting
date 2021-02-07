using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed = 6.0f;

    public float jumpSpeed = 8.0f;

    public float gravity = 20.0f;

    public float rotateSpeed = 3.0f;

    private CharacterController characterController;

    private Vector3 moveDirection = Vector3.zero;

    private float h, v;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        if(characterController.isGrounded)
        {
            gameObject.transform.Rotate(new Vector3(0, rotateSpeed * h, 0));
            moveDirection = speed * v * gameObject.transform.forward;
        }

        if(Input.GetButton("Jump"))
        {
            moveDirection.y = jumpSpeed;
        }

        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);
    }
}
