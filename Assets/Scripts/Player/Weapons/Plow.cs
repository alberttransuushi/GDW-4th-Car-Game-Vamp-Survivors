using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plow : PrimaryWeapon
{
    [SerializeField] Vector3 startingSize;
    [SerializeField] float sizeMultiplierOnActive;
    [SerializeField] float damage;
    [SerializeField] float knockBackStrength;

    [SerializeField] float stunDuration;

    [SerializeField] float sizeUpgradeIncrease;
    [SerializeField] float reloadUpgradeDecrease;


    int refundPerKill;
    [SerializeField] int killRefundUpgradeIncrease;


    bool isFiring;
    BoxCollider BoxCollider;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        startingSize = gameObject.transform.localScale;
        BoxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    public override void Update()
    {
        if (fireControl.action.IsPressed() && currentAmmo > 0)
        {
            Fire();

        } else if(fireControl.action.IsPressed() && currentAmmo <= 0)
        {
            StartCoroutine(Reload());
        } 
        else
        {
            gameObject.transform.localScale = startingSize;
        }

        if (reloadControl.action.WasPressedThisFrame() && !isReloading) StartCoroutine(Reload());

        BoxCollider.size = BoxCollider.size;
    }

    public override void Fire()
    {
        if (currentAmmo > 0)
        {
            IncreaseSize();

        }
        else if (!isReloading) StartCoroutine(Reload());
    }

    void IncreaseSize()
    {
        gameObject.transform.localScale = startingSize * sizeMultiplierOnActive;
        currentAmmo--; ;
    }



    
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(collision.gameObject.GetComponent<BaseEnemy>().Stun(stunDuration));
            collision.gameObject.GetComponent<BaseEnemy>().takeDamge(damage);
            Vector3 knockbackVector = (collision.transform.position - transform.position).normalized * knockBackStrength;
            collision.gameObject.GetComponent<Rigidbody>().AddForce(knockbackVector, ForceMode.Impulse);
            collision.gameObject.GetComponent<Rigidbody>().AddForce(collision.transform.up * knockBackStrength / 1.5f, ForceMode.Impulse);
            if(player.gameObject.GetComponent<PlayerCar>().currentHP < player.gameObject.GetComponent<PlayerCar>().maxHP)
            {

                player.gameObject.GetComponent<PlayerCar>().TakeDamage(-damage * 2);
            }
            currentAmmo += refundPerKill;
        }
    }

    protected override void Upgrade1()
    {
        sizeMultiplierOnActive += sizeUpgradeIncrease;
    }
    protected override void Upgrade2()
    {
        

        reloadTime -= reloadUpgradeDecrease;
        
    }
    protected override void Upgrade3()
    {
        refundPerKill += killRefundUpgradeIncrease;
    }
}
