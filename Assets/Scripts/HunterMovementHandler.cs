using UnityEngine;
using System.Collections;

public class HunterMovementHandler : MonoBehaviour {

    Rigidbody rigidbody;
    //MOVE
    public float MoveSpeed = 10;
    public float Gravity = 9.8f;
    public Vector3 moveDirection;
    //ROTATION
    public float speedH = 2.0f;
    public float speedV = 2.0f;
    //JUMP
    public float Jump = 10f;
    public bool jumping = false;
    //SHOT
    public GameObject BulletSpawn;
    public GameObject BulletPrefab;
    private float nextBullet = 0f;
    public float BulletSpeed = 10f;
    AudioSource source;
    public AudioClip ShootSound;
    public GameObject BulletHole;


    public float horizontalAngle = 270.0f;
    public float verticalAngle = 0.0f;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }
    // Use this for initialization
    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        Move();
        Shoot();
    }

    private void Move()
    {
        rigidbody.AddForce(Vector3.up * -10);
        if (jumping)
            return;
        //Set moveDirection to the vertical axis (up and down keys) * speed
        moveDirection = new Vector3(MoveSpeed * Input.GetAxis("Horizontal"), -Gravity, MoveSpeed * Input.GetAxis("Vertical"));
        //Transform the vector3 to local space
        moveDirection = transform.TransformDirection(moveDirection);
        //set the velocity, so you can move
        rigidbody.velocity = new Vector3(moveDirection.x, /*moveDirection.y*/rigidbody.velocity.y, moveDirection.z);
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
        transform.eulerAngles = new Vector3(0f, 270 - horizontalAngle, 0.0f);
        Camera.main.transform.eulerAngles = new Vector3(verticalAngle, Camera.main.transform.eulerAngles.y,
            Camera.main.transform.eulerAngles.z);
    }

    void Shoot()
    {
        nextBullet -= Time.deltaTime;
        if (Input.GetAxis("Fire1") > 0 && nextBullet <= 0)
        {
            //GameObject bullet = Instantiate(BulletPrefab, BulletSpawn.transform.position, BulletSpawn.transform.rotation) as GameObject;
            //bullet.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * BulletSpeed, ForceMode.VelocityChange);
            source.PlayOneShot(ShootSound);
            RaycastHit hit;
            if (Physics.Raycast(BulletSpawn.transform.position, Camera.main.transform.forward, out hit, 100.0f))
            {
                GameObject bulletHole = Instantiate(BulletHole, hit.point + (hit.normal * 0.01f),
                    Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject;
                //BulletHole.transform.parent = hit.transform;
                if(hit.transform.tag=="PropPlayer")
                {
                    HealthScript healt = hit.transform.gameObject.GetComponent<HealthScript>();
                    healt.TakeDemage(21);
                }
                Destroy(bulletHole, 3);
            }
            nextBullet = Random.Range(0.09f, 0.2f);

        }
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
