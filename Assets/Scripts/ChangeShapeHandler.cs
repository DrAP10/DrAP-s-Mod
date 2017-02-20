using UnityEngine;
using System.Collections;

public class ChangeShapeHandler : MonoBehaviour {

    public GameObject bookPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyDown(KeyCode.E))
        {
            
            RaycastHit hit;
            GameObject shape = gameObject.transform.FindChild("shape").gameObject;
            //EnemyLayer is the layer having enemys
            Debug.DrawRay(shape.transform.position, shape.transform.forward,Color.red, 20, true);
            print("E");
            if (Physics.Raycast(shape.transform.position, shape.transform.forward, out hit, 1.75f))
            {
                print(hit.collider.transform.name);
                PrefabScript ps = hit.collider.gameObject.GetComponent<PrefabScript>();
                if (ps != null)
                {
                    GameObject prefab = ps.prefab;
                    GameObject g = Instantiate(prefab, shape.transform.position, new Quaternion(0, 180, 0, 0)) as GameObject;
                    g.transform.parent = transform;
                    //Camera.main.transform.parent = g.transform;
                    g.name = "shape";
                    Destroy(g.GetComponent<Rigidbody>());
                    Destroy(shape);
                }
            }
        }
	}
}
