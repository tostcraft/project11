using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]

public class shootingRifle : MonoBehaviour
{

    private RaycastHit hit;
    private float range = 10.0f;
    private GameObject pistolSparks;
    private Vector3 fwd;
    public int maxClips = 36;
    public int clipSize = 10;
    private int currentClips = 3;
    private int currentAmmo;
    private bool isReloading = false;
    private float timer = 0.0f;
    private bool is_shooting = false;
    private float shootTimer = 0;
    [SerializeField] public GameObject bullet;
    

    
    public AudioClip pistolShot;
    public AudioClip reloadSound;
    public Text ammoText;
    public Text reloadText;
    public GameObject pointer;
    public float shoots_offset;
    [SerializeField]public float reloadTime = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;

        pistolSparks = GameObject.Find("Sparks");
        GetComponent<AudioSource>().clip= pistolShot;
        currentAmmo = clipSize;
    }

    void OnGUI()
    {
//        ammoText.pixelOffset= new Vector2(-Screen.width/2+100, -Screen.height / 2 + 30);
        ammoText.text=currentAmmo+"/"+currentClips;
    }

    // Update is called once per frame
    void Update()
    {
        fwd = transform.forward;

        if(Input.GetButtonDown("Fire1") && currentAmmo>0 && !isReloading)
        {
            is_shooting = true;
        }
        if(is_shooting && currentAmmo >0 && !isReloading)
        {
            
            /*    if(Physics.Raycast(transform.position, fwd, out hit, range))
                {
                    if(hit.transform.tag == "Enemy")
                    {
                        Debug.Log("Trafiony przeciwnik");
                    }
                }
                else
                {
                    Debug.Log("Trafiona ściana");
                }*/
            shootTimer += Time.deltaTime;
            if (shootTimer >= shoots_offset)
            {
                currentAmmo--;
//                pistolSparks.GetComponent<ParticleSystem>().Play();
                GetComponent<AudioSource>().Play();
                GameObject bulletClone = Instantiate(bullet, pointer.GetComponent<Transform>().position, transform.rotation);
                Destroy(bulletClone, 5);
                shootTimer = 0;
            }
            
        }
        if (Input.GetButtonUp("Fire1") && is_shooting)
        {
            is_shooting = false;
        }
        if(Input.GetButtonDown("Fire1") && currentAmmo==0 || Input.GetButtonDown("Reload") && currentAmmo<clipSize)
        {
            if(currentClips>0)
            {
                GetComponent<AudioSource>().clip = reloadSound;
                GetComponent<AudioSource>().Play();
                isReloading = true;

            }

        }
        if(isReloading)
        {
            timer+=Time.deltaTime;
            if(timer>=reloadTime)
            {
                currentAmmo = clipSize;
                currentClips-=1;
                GetComponent<AudioSource>().clip=pistolShot;
                isReloading = false;
                timer = 0.0f;
            }
                    
        }
        if (currentAmmo == 0)
        {
            reloadText.enabled = true;
        }
        else
        {
            reloadText.enabled = false;
        }
    }
    /// <summary>
    /// Callback to draw gizmos that are pickable and always drawn.
    /// </summary>
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.TransformDirection(Vector3.right));
    }
    void magazinePack()
    {
        currentClips+=3;
    }
}
