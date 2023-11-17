using ND_VariaBULLET;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFireRateBehaviour : MonoBehaviour
{
    [Tooltip("Set time (in seconds) of a single burst time lenght")]
    [SerializeField] private float fireLenght;

    [Tooltip("Set time (in seconds) of each pause between bursts")]
    [SerializeField] private float pauseBurstLenght;

    [Tooltip("Set time (in seconds) for delaying the first burst after instantiation")]
    [SerializeField] private float delayedStartFire;

    private SpreadPattern SpreadPattern;

    private float fireTimer = 0;
    private float pauseBurstTimer = 0;
    private float delayedStartFireTimer = 0;

    private bool _firePaused;
    private bool firePaused 
    { 
        get 
        { 
            return _firePaused; 
        } 
        set 
        {  
            _firePaused = value;
            SpreadPattern.TriggerAutoFire = !value;
        }
    }

    // Start is called before the first frame update
    private void Awake()
    {

        SpreadPattern = GetComponent<SpreadPattern>();
        _firePaused = true;
    }

    void Update()
    {
        if (!DelayFirstFire())
        {
            FireCycle();
        }
    }

    private bool DelayFirstFire()
    {
        if (delayedStartFireTimer <= delayedStartFire)
        {
            delayedStartFireTimer += Time.deltaTime;
            return true;
        }
        //firePaused = false;
        return false;
    }

    private void FireCycle()
    {
        if(firePaused)
        {
            pauseBurstTimer += Time.deltaTime;
            if(pauseBurstTimer >= pauseBurstLenght)
            {
                firePaused = false;
                pauseBurstTimer = 0;
            }
        }
        else
        {
            fireTimer += Time.deltaTime;
            if (fireTimer >= fireLenght)
            {
                firePaused = true;
                fireTimer = 0;
            }
        }
    }


}
