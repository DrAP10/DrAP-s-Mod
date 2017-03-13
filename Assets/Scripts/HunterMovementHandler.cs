using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class HunterMovementHandler : NetworkBehaviour {

    Rigidbody rigidbody;
    //GUI
    GUIStyle guiStyle;
    public Texture2D HeartTexture;
    //MOVE
    public float MoveSpeed = 10;
    public float Gravity = 9.8f;
    public Vector3 moveDirection;
    //ROTATION
    public float speedH = 2.0f;
    public float speedV = 2.0f;
    Transform gun;
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

    public GameObject PropPlayer;


    public float horizontalAngle = 270.0f;
    public float verticalAngle = 0.0f;

    /*public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("OnStartServer " + connectionToClient + " " + isLocalPlayer);

    }
    public override void OnStartClient()
    {
        base.OnStartClient();
        Debug.Log("OnStartClient " + connectionToClient + " " + isLocalPlayer);
    }*/
    void Awake()
    {
        /*if (!gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
            return;
        source = GetComponent<AudioSource>();*/
    }
    // Use this for initialization
    void Start()
    {

        if (!gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
        {
            transform.FindChild("Gun").FindChild("Camera").gameObject.SetActive(false);
            //return;
        }
        rigidbody = gameObject.GetComponent<Rigidbody>();
        gun = gameObject.transform.FindChild("Gun");
        source = GetComponent<AudioSource>();
        guiStyle = new GUIStyle();
        guiStyle.normal.textColor = Color.black;
        guiStyle.fontSize = 20;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
            return;
        Rotate();
        Move();

        nextBullet -= Time.deltaTime;
        if (Input.GetAxis("Fire1") > 0 && nextBullet <= 0)
        {
            CmdShoot();
            nextBullet = Random.Range(0.09f, 0.2f);
        }
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
        gun.eulerAngles = new Vector3(verticalAngle, gun.eulerAngles.y,
            gun.eulerAngles.z);
    }

    [Command]
    void CmdShoot()
    {
        //GameObject bullet = Instantiate(BulletPrefab, BulletSpawn.transform.position, BulletSpawn.transform.rotation) as GameObject;
        //bullet.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * BulletSpeed, ForceMode.VelocityChange);
        //source.PlayOneShot(ShootSound);
        RaycastHit hit;
        if (Physics.Raycast(BulletSpawn.transform.position, gun.forward, out hit, 100.0f))
        {
            
            if (hit.transform.tag == "PropPlayer")
            {
                HealthScript healt = hit.transform.gameObject.GetComponent<HealthScript>();
                healt.TakeDemage(21);
            }
            else if (hit.transform.tag == "HunterPlayer")
            {
                HealthScript healt = hit.transform.gameObject.GetComponent<HealthScript>();
                healt.TakeDemage(21);
            }
            else
            {
                GameObject bulletHole = Instantiate(BulletHole, hit.point + (hit.normal * 0.01f),
                                Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject;
                NetworkServer.Spawn(bulletHole);
                //BulletHole.transform.parent = hit.transform;
                Destroy(bulletHole, 3);
            }
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

    void OnGUI()
    {
        if (!gameObject.GetComponent<NetworkIdentity>().isLocalPlayer)
            return;
        GUI.Label(new Rect(10, 10, 30, 30), HeartTexture, guiStyle);
        GUI.Label(new Rect(10 + 30, 10, 100, 20), GetComponent<HealthScript>().Health.ToString(), guiStyle);
    }
}
