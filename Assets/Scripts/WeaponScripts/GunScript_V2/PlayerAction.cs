using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAction : MonoBehaviour
{
    [SerializeField]
    private GunScript_V2 Gun;
    [SerializeField]
    private InputActionAsset InputActions;
    private void OnEnable()
    {
        foreach (InputAction action in InputActions.actionMaps[0].actions)
        {
            if (action.name.Equals("Shoot"))
            {
                action.performed += HandleShoot;
            }
        }
    }

    private void OnDisable()
    {
        foreach (InputAction action in InputActions.actionMaps[0].actions)
        {
            if (action.name.Equals("Shoot"))
            {
                action.performed -= HandleShoot;
            }
        }
    }

    private void HandleShoot(InputAction.CallbackContext obj)
    {
        if (obj.performed)
        {
            Gun.Shoot();
        }
    }

    public void OnShoot()
    {
        Gun.Shoot();
    }
}