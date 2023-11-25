using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] private GameObject RetryButton;

    private void FixedUpdate()
    {
        if(GameInfo.IsPlayerAlive == false && RetryButton.activeSelf == false)
        {
            RetryButton.SetActive(true);
        }
    }

}
