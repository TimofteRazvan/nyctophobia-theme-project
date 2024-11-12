using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telebizor : MonoBehaviour
{
    [SerializeField] GameObject Tv_on;
    [SerializeField] GameObject midpoint;
    [SerializeField] GameObject lumina_auntru;
    [SerializeField] GameObject lumina_afara;


    // Start is called before the first frame update
    void Start()
    {
        midpoint = FindObjectOfType<Gol>().gameObject;
        Tv_on = FindObjectOfType<TelebizorPornit>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Telc")
        {
            if (Input.GetKey(KeyCode.E))
            {
                GameObject tv_on = Instantiate(Tv_on, this.transform);
                tv_on.transform.parent = midpoint.transform;
                tv_on.transform.position = this.transform.position;
                lumina_auntru.gameObject.SetActive(false);
                lumina_afara.gameObject.SetActive(true);
                Destroy(this.gameObject);
            }
            
        }
    }
}
