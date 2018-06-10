using UnityEngine;

public class PlayerStats : MonoBehaviour {

    [Header("Player Stats")]
    [SerializeField]
    private float health = 100;
    public float Health
    {
        get { return health; }
        set {
            if (value > 100) health = 100;
            else if (value < 0) health = 0;
            else health = value;
        }
    }
    [SerializeField]
    private float hunger = 100;
    public float Hunger
    {
        get { return hunger; }
        set
        {
            if (value > 100) hunger = 100;
            else if (value < 0) hunger = 0;
            else hunger = value;
        }
    }
    [SerializeField]
    private float thirst = 100f;
    public float Thirst
    {
        get { return thirst; }
        set
        {
            if (value > 100f) thirst = 100;
            if (value < 0) thirst = 0;
            else thirst = value;
        }
    }
    [SerializeField]
    private float oxygen = 100f;
    public float Oxygen
    {
        get { return oxygen; }
        set
        {
            if (value > 100f) oxygen = 100;
            else if (value < 0) oxygen = 0;
            else oxygen = value;
        }
    }
    [SerializeField]
    private float sleep = 100f;
    public float Sleep
    {
        get { return sleep; }
        set
        {
            if (value > 100f) sleep = 100;
            else if (value < 0) sleep = 0;
            else sleep = value;
        }
    }

    [Header("Decrease Rates")]
    public float hungerRate = 5f; //0.5 
    public float thirstRate = 5f; //0.5
    public float oxygenRate = 10f;//2
    public float sleepRate = 10f;//1

    private float totalDamage = 0;

    public PlayerController controller;
    
    void Start()
    {
        controller = GetComponent<PlayerController>();
        InvokeRepeating("LoopAllStats", 5, 5);
    }

    public void Save()
    {
        Debug.Log("Saving player data...");
        SaveLoadManager.SavePlayer(this);
    }

    public void Load()
    {
        Debug.Log("Loading player data...");
        PlayerData data = SaveLoadManager.LoadPlayer();
        if (data != null)
        {
            gameObject.SetActive(false);

            Health = data.health;
            Hunger = data.hunger;
            Oxygen = data.oxygen;
            Sleep = data.sleep;

            controller.insideBase = data.inBase;

            hungerRate = data.hungerRate;
            oxygenRate = data.oxygenRate;
            sleepRate = data.sleepRate;

            MainUIManager.Instance.UpdateStatsPanel(Health, Oxygen, Hunger, Thirst, Sleep);

            transform.position = new Vector3(data.posX, data.posY, data.posZ);
            transform.rotation = Quaternion.Euler(data.rotX, data.rotY, data.rotZ);

            gameObject.SetActive(true);
        }
    }

    public void Heal(float _extraHealth)
    {
        Health += _extraHealth;
    }

    public void Damage(float _damage)
    {
        Health -= _damage;
        if (Health == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        //Debug.Log("You died :'(");
    }

    private void LoopAllStats()
    {
        totalDamage = 0;

        DecreaseHunger();
        DecreaseThirst();
        DecreaseOxygen();
        DecreaseSleep();

        if (totalDamage > 0)
        {
            Damage(totalDamage);
        }
        else
        {
            Heal(5f);
        }

        Damage(totalDamage);

        MainUIManager.Instance.UpdateStatsPanel(Health, Oxygen, Hunger, Thirst, Sleep);
    }

    private void DecreaseHunger()
    {
        Hunger -= hungerRate;
        if (Hunger == 0)
        {
            totalDamage += 5f;
        }
        /*if (Hunger < 80f)
        {
            controller.walkSpeed = 2f;
        }*/
    }

    private void DecreaseThirst()
    {
        Thirst -= thirstRate;
        if (Thirst == 0)
        {
            totalDamage += 10f;
        }
        /*if (Thirst < 80f)
        {
            controller.walkSpeed = 2f;
        }*/
    }

    private void DecreaseOxygen()
    {
        Oxygen -= oxygenRate;
        if (Oxygen == 0)
        {
            totalDamage += 0.5f;
        }
    }

    private void DecreaseSleep()
    {
        Sleep -= sleepRate;
        if (Sleep == 0)
        {
            //Debug.Log("Falling asleep...");
        }
    }
}
