using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController characterController;
    private Vector3 Velocity;
    public Transform verRot;
    public Transform horRot;
    public float MoveSpeed;
	private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
		anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
		float X_Rotation = Input.GetAxis("Mouse X");
		float Y_Rotation = Input.GetAxis("Mouse Y");
		horRot.transform.Rotate(new Vector3(0, X_Rotation * 2, 0));
		verRot.transform.Rotate(-Y_Rotation * 2, 0, 0);

		if (Input.GetKey(KeyCode.W))
		{
			characterController.Move(this.gameObject.transform.forward * MoveSpeed * Time.deltaTime);
			anim.SetBool("is_running", true);
		}

		else if (Input.GetKeyUp(KeyCode.W))
		{
			anim.SetBool("is_running", false);
		}

		if (Input.GetKey(KeyCode.S))
		{
			characterController.Move(this.gameObject.transform.forward * -1f * MoveSpeed * Time.deltaTime);
			anim.SetBool("is_running", true);
		}

		else if (Input.GetKeyUp(KeyCode.S))
		{
			anim.SetBool("is_running", false);
		}

		if (Input.GetKey(KeyCode.A))
		{
			characterController.Move(this.gameObject.transform.right * -1 * MoveSpeed * Time.deltaTime);
			anim.SetBool("is_running", true);
		}

		else if (Input.GetKeyUp(KeyCode.A))
		{
			anim.SetBool("is_running", false);
		}

		if (Input.GetKey(KeyCode.D))
		{
			characterController.Move(this.gameObject.transform.right * MoveSpeed * Time.deltaTime);
			anim.SetBool("is_running", true);
		}
		
		else if (Input.GetKeyUp(KeyCode.D))
		{
			anim.SetBool("is_running", false);
		}

		characterController.Move(Velocity);
		Velocity.y += Physics.gravity.y * Time.deltaTime;
	}
}
