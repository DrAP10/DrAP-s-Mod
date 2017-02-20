using UnityEngine;
using System.Collections;

public class PropMovementHandler : MonoBehaviour {

    Rigidbody rigidbody;
    //MOVE
    public float MoveSpeed=10;
    public float Gravity=9.8f;
    public Vector3 moveDirection;
    //ROTATION
    public float speedH = 2.0f;
    public float speedV = 2.0f;
    //JUMP
    public float Jump = 10f;
    public bool jumping = false;


    public float horizontalAngle = 270.0f;
    public float verticalAngle = 0.0f;

    // Use this for initialization
    void Start () {
        rigidbody = gameObject.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        Rotate();
        Move();
    }

    private void Move()
    {
        rigidbody.AddForce(Vector3.up * -10);
        if (jumping)
            return;
        //Set moveDirection to the vertical axis (up and down keys) * speed
        moveDirection = new Vector3(MoveSpeed * Input.GetAxis("Horizontal"), /*(jumping)?0:*/-Gravity, MoveSpeed * Input.GetAxis("Vertical"));
        //Transform the vector3 to local space
        Transform camera = Camera.main.transform;
        camera.eulerAngles = new Vector3(0f, camera.eulerAngles.y,
            camera.eulerAngles.z);
        moveDirection = camera.TransformDirection(moveDirection);
        camera.eulerAngles = new Vector3(20f, camera.eulerAngles.y,
            camera.eulerAngles.z);
        //set the velocity, so you can move
        rigidbody.velocity = new Vector3(moveDirection.x, /*moveDirection.y*/rigidbody.velocity.y, moveDirection.z);
        //rigidbody.AddForce(moveDirection, ForceMode.Acceleration);
        if (Input.GetAxis("Jump") > 0)
        {
            rigidbody.AddForce(Vector3.up * Jump, ForceMode.Impulse);
            jumping = true;
        }
    }

    private void Rotate()
    {
        horizontalAngle -= speedH * Input.GetAxis("Mouse X");
        verticalAngle -= speedV * Input.GetAxis("Mouse Y");
        //print("Angle: " + verticalAngle + ", sin: " + Mathf.Sin(verticalAngle));
        Camera.main.transform.localPosition = new Vector3(Mathf.Cos((horizontalAngle * Mathf.PI) / 180) * 2.5f,
            2f,//Mathf.Sin((verticalAngle * Mathf.PI) / 180),
            Mathf.Sin((horizontalAngle * Mathf.PI) / 180) * 2.5f);
        Camera.main.transform.eulerAngles = new Vector3(20f, 270 - horizontalAngle, 0.0f);
        //Camera.main.transform.eulerAngles = RotatePointAroundPivot(transform.position, 
        //  gameObject.transform.FindChild("shape").position, new Vector3(pitch, yaw, 0.0f));
    }

    public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }

    void OnCollisionEnter(Collision collision)
    {
        jumping = false;
    }

    void OnCollisionLeave(Collision collision)
    {
        jumping = true;
    }
    
}
