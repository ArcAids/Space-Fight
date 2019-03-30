using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : IControlPlayer , IControlShooting
{
    public static bool inputEnabled=false;
    public float Horizontal { get; private set; }

    public bool Fire { get; private set; }

    public void ReadInput()
    {
        if (!inputEnabled)
            return;

        if (Application.platform == RuntimePlatform.Android)
        {
            Horizontal = Input.acceleration.x;
            Fire = Input.GetMouseButtonDown(0);
        }
        else
        {
            Horizontal = Input.GetAxis("Horizontal");
            Fire = Input.GetButtonDown("Fire1");
        }
    }
}

public interface IControlPlayer
{
    float Horizontal { get; }

    void ReadInput();
}

public interface IControlShooting
{
    bool Fire { get; }

    void ReadInput();
}