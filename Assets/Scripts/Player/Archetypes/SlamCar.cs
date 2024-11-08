using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlamCar : MonoBehaviour
{
    // Start is called before the first frame update
    enum SlamType
    {
        BasedOnCurrent,
        StaticSlamSpeed

    }

    [SerializeField] SlamType slamType;

    [SerializeField] float StaticSlamSpeed;

    [SerializeField]
    private InputActionReference UnstuckControl;

    public bool CanSlam;

    PlayerCar player;

    private void OnEnable()
    {
        UnstuckControl.action.Enable();
    }
    private void OnDisable()
    {
        UnstuckControl.action.Disable();
    }

    private void Start()
    {
        player = GetComponent<PlayerCar>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.CheckGrounded())
        {
            if (!CanSlam)
            {
                player.Explode();
                CanSlam = true;
            }

            
        }

        if (UnstuckControl.action.triggered && CanSlam && !player.CheckGrounded())
        {
            CanSlam = false;

            print("SLam");
            if(slamType == SlamType.BasedOnCurrent)
            {

                player.GetComponent<Rigidbody>().velocity = new Vector3(0, -Vector3.Magnitude(player.GetComponent<Rigidbody>().velocity), 0);

            } else if (slamType == SlamType.StaticSlamSpeed)
            {

                player.GetComponent<Rigidbody>().velocity = new Vector3(0, -StaticSlamSpeed, 0);

            }


        }
    }
}
