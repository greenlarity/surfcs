using System;
using UnityEngine;

public class InputData
{
    public float ForwardMove;
    public float SideMove;
    public float MouseX;
    public float MouseY;
    public bool JumpPressed;
    public bool JumpPressedLastUpdate;
    public bool ResetPressed;

    public void Update(MovementConfig movementConfig)
    {
        #region ---- Catch Movement ----

        var moveLeft = Input.GetKey(movementConfig.MoveLeft);
        var moveRight = Input.GetKey(movementConfig.MoveRight);
        var moveForward = Input.GetKey(movementConfig.MoveForward);
        var moveBack = Input.GetKey(movementConfig.MoveBack);

        // Sides movement
        if (moveLeft)
        {
            SideMove = -movementConfig.Accel;
        }
        else if (moveRight)
        {
            SideMove = movementConfig.Accel;
        }
        else
        {
            SideMove = 0;
        }

        //Frontal movement
        if (moveForward)
        {
            ForwardMove = movementConfig.Accel;
        }
        else if (moveBack)
        {
            ForwardMove = -movementConfig.Accel;
        }
        else
        {
            ForwardMove = 0;
        }

        #endregion

        #region ---- Catch Inputs ----

        ResetPressed = Input.GetKey(movementConfig.ResetButton);

        JumpPressedLastUpdate = JumpPressed;
        JumpPressed = Input.GetKey(movementConfig.JumpButton);

        #endregion

        #region ---- Catch Mouse Movement ----

        MouseX = Input.GetAxis("Mouse X") * movementConfig.XSens * 0.02f;
        MouseY = Input.GetAxis("Mouse Y") * movementConfig.YSens * 0.02f;

        #endregion
    }

    public void CalculateMovement(MovementConfig movementConfig)
    {
        #region ---- Catch Movement ----

        var moveLeft = Input.GetKey(movementConfig.MoveLeft);
        var moveRight = Input.GetKey(movementConfig.MoveRight);
        var moveForward = Input.GetKey(movementConfig.MoveForward);
        var moveBack = Input.GetKey(movementConfig.MoveBack);

        // Sides movement
        if (moveLeft)
        {
            SideMove = -movementConfig.Accel;
        }
        else if (moveRight)
        {
            SideMove = movementConfig.Accel;
        }
        else
        {
            SideMove = 0;
        }

        //Frontal movement
        if (moveForward)
        {
            ForwardMove = movementConfig.Accel;
        }
        else if (moveBack)
        {
            ForwardMove = -movementConfig.Accel;
        }
        else
        {
            ForwardMove = 0;
        }

        #endregion

        #region ---- Catch Inputs ----

        ResetPressed = Input.GetKey(movementConfig.ResetButton);

        JumpPressedLastUpdate = JumpPressed;
        JumpPressed = Input.GetKey(movementConfig.JumpButton);

        #endregion

        #region ---- Catch Mouse Movement ----

        MouseX = Input.GetAxis("Mouse X") * movementConfig.XSens * 0.02f;
        MouseY = Input.GetAxis("Mouse Y") * movementConfig.YSens * 0.02f;

        #endregion
    }
}