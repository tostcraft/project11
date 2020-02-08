using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class groundWeapon : MonoBehaviour
{

    public int weaponNumber;
    public Text pickupText;

    private bool isPlayer;

    void OnTriggerStay(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            pickupText.enabled = true;
            isPlayer = true;
            if(Input.GetKeyDown(KeyCode.F))
            {
                Destroy(gameObject);
                other.gameObject.SendMessage("addGun", weaponNumber);
                
                pickupText.enabled = false;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            isPlayer = false;
            pickupText.enabled = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        pickupText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
