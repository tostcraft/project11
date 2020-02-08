using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponSwitch : MonoBehaviour
{
    private KeyCode[] keys = new KeyCode[] {KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3 };
    private bool[] guns = new bool[] { false, true, false, false};
    private int maxGuns = 0;
    public GameObject player;

    public int addGun(int number)
    {
        if(!guns[number])
        {
            guns[number] = true;
            maxGuns++;
            Debug.Log(maxGuns);
        }
        return 0;
    }
    public GameObject[] gunList = new GameObject[3]; 

    // Start is called before the first frame update
    void Start()
    {
        player.SendMessage("switchWeapon", gunList[1]);

    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i<keys.Length; i++)
        {
            if(Input.GetKeyDown(keys[i]) && guns[i])
            {
                hideGuns();
                gunList[i].SetActive(true);
                player.SendMessage("switchWeapon", gunList[i]);
            }
        }
    }

    private void hideGuns()
    {
        for(int i = 1; i <= maxGuns+1; i++)
        {
            gunList[i].SetActive(false);
        }
    }
    
}
