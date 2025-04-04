using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadLights : PrimaryWeapon
{
    public float damageConeAngle;
    public float damage;

    float damageTimer;
    public float damageCooldown;


    [SerializeField] float sizeUpgradeIncrease;
    [SerializeField] float rangeUpgradeIncrease;
    [SerializeField] float damageUpgradeIncrease;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward * 100f, Color.red, 0.5f);

        enemyArray = enemyTracker.GetEnemyArray();


        if (fireControl.action.IsPressed())
        {
            Fire();

        }

        if (reloadControl.action.WasPressedThisFrame() && !isReloading) StartCoroutine(Reload());

        damageTimer -= Time.deltaTime;
    }

    public override void Fire()
    {
        if (currentAmmo > 0)
        {
            DamageAffectedEnemies();

        }
        else if (!isReloading) StartCoroutine(Reload());
    }

    public void DamageAffectedEnemies()
    {
        ///If cooldown is done do this
        
        if(damageTimer <= 0f)
        {
            foreach (GameObject enemy in enemyArray)
            {
                if (Vector3.Angle(Vector3.Normalize(enemy.transform.position - transform.position), transform.forward) <= damageConeAngle && Vector3.Distance(enemy.transform.position, transform.position) <= maxRange && Vector3.Distance(enemy.transform.position, transform.position) >= minRange)
                {
                    BaseEnemy baseEnemy = enemy.GetComponent<BaseEnemy>();
                    baseEnemy.takeDamge(damage);
                    Debug.Log("Damage Enemy");
                }



            }
            currentAmmo--;
            damageTimer = damageCooldown;
        }
        
    }

    protected override void Upgrade1()
    {
        damageConeAngle += sizeUpgradeIncrease;
    }
    protected override void Upgrade2()
    {
        

        maxRange -= rangeUpgradeIncrease;
        
    }
    protected override void Upgrade3()
    {
        damage += damageUpgradeIncrease;
    }

}
