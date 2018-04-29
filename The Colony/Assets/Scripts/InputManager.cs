using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    private const string H_AXIS = "Horizontal";
    private const string V_AXIS = "Vertical";
    private const string MOUSE_X = "Mouse X";
    private const string MOUSE_Y = "Mouse Y";
    private const string JUMP = "Jump";
    private const string CANCEL = "Cancel";
    private const string INTERACT = "Interact";

    public float x_AxisMouse;
    public float y_AxisMouse;
    public float v_Axis;
    public float h_Axis;
    public bool l_Shift;
    public bool jump;
    public bool interact;
    public bool cancel;
    public bool buildMenu;

    // Update is called once per frame
    void Update () {
        x_AxisMouse = Input.GetAxis(MOUSE_X);
        y_AxisMouse = Input.GetAxis(MOUSE_Y);

        v_Axis = Input.GetAxis(V_AXIS);
        h_Axis = Input.GetAxis(H_AXIS);

        l_Shift = Input.GetKey(KeyCode.LeftShift);
        jump = Input.GetButton(JUMP);

        interact = Input.GetButtonDown(INTERACT);
        cancel = Input.GetButtonDown(CANCEL);
        buildMenu = Input.GetKeyDown(KeyCode.B);
    }
}
