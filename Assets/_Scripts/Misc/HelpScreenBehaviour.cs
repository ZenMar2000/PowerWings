using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class HelpScreenBehaviour : MonoBehaviour
{
    private void OnEnable()
    {
        InputManager.HideHelp.performed += ShowHideHelp;

    }

    private void OnDestroy()
    {
        InputManager.HideHelp.performed -= ShowHideHelp;

    }

    private void ShowHideHelp(InputAction.CallbackContext context)
    {
       gameObject.SetActive(!gameObject.activeSelf);
       GameInfo.HelpScreenVisible = gameObject.activeSelf;
    }
}
