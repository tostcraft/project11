using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private float maxHealth = 100;
    private float currentHealth=100;
    private float maxArmour=100;
    private float currentArmour=100;
    private float maxStamina=300;
    private float currentStamina=300;
    private float canHeal = 0.0f;
	private float canRegenerate = 0.0f;
    
    public Texture2D healthTexture;
    public Texture2D armourTexture;
    public Texture2D staminaTexture;
    
    private float barWidth;
    private float barHeight;
    private CharacterController chCont;
    private UnityStandardAssets.Characters.FirstPerson.FirstPersonController fpsC;
    private Vector3 lastPosition;
    private GameObject currentWeapon;
    
    public float walkSpeed = 10.0f;
	public float runSpeed = 20.0f;


    //Awake is called when instance is created
    void Awake()
    {
        barHeight=Screen.height*0.02f;
        barWidth=barHeight*10.0f;
        chCont=GetComponent<CharacterController>();
        fpsC=gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>();
        lastPosition = transform.position;
    }

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(Screen.width - barWidth - 10,
                          Screen.height - barHeight - 10,
                          currentHealth * barWidth / maxHealth,
                          barHeight),
                        healthTexture);
        GUI.DrawTexture(new Rect(Screen.width - barWidth - 10,
                          Screen.height - barHeight * 2 - 20,
                          currentArmour * barWidth / maxArmour,
                          barHeight),
                        armourTexture);
        GUI.DrawTexture(new Rect(Screen.width - barWidth - 10,
                          Screen.height - barHeight * 3 - 30,
                          currentStamina * barWidth / maxStamina,
                          barHeight),
                        staminaTexture);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            takeHit(30);
        }
        if(canHeal > 0.0f) {
			canHeal -= Time.deltaTime;
		}
		if(canRegenerate > 0.0f) {
			canRegenerate -= Time.deltaTime;
		}

		if(canHeal <= 0.0f && currentHealth < maxHealth) {
			regenerate(ref currentHealth, maxHealth);
		}
		if(canRegenerate <= 0.0f && currentStamina < maxStamina) {
			regenerate(ref currentStamina, maxStamina);
		}
    }

    void FixedUpdate ()
    {
        float speed=walkSpeed;
        if(chCont.isGrounded && Input.GetKey(KeyCode.LeftShift) && lastPosition!=transform.position && currentStamina>0)
        {
            lastPosition=transform.position;
            speed=runSpeed;
            currentStamina-=1;
            currentStamina=Mathf.Clamp(currentStamina, 0, maxStamina);
            canRegenerate=5.0f;
        }
        if(currentStamina > 0)
        {
            fpsC.CanRun=true;
        }
        else
        {
            fpsC.CanRun=false;
        }
    }
    void takeHit(float damage)
    {
        if(currentArmour>0)
        {
            currentArmour-=damage;
        }
        if(currentArmour<0)
        {
            currentHealth+=currentArmour;
            currentArmour=0;
        }
        if(currentArmour==0)
        {
            currentHealth-=damage;
        }

        currentArmour = Mathf.Clamp(currentArmour, 0, maxArmour);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);       
    }
    void regenerate(ref float currentStat, float maxStat)
	{
		currentStat += maxStat * 0.005f;
		Mathf.Clamp(currentStat, 0, maxStat);
	}
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "ammoPack")
        {
            Debug.Log("ammo pack collide");
            Destroy(other.gameObject);
            currentWeapon.SendMessage("magazinePack");
            
        }
    }

    void switchWeapon(GameObject newWeapon)
    {
        currentWeapon = newWeapon;
    }
}
