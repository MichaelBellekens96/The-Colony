using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    private const string H_AXIS = "Horizontal";
    private const string V_AXIS = "Vertical";
    private const string MOUSE_X = "Mouse X";
    private const string MOUSE_Y = "Mouse Y";
    private const string MOUSE_0 = "Fire1";
    private const string JUMP = "Jump";
    private const string CANCEL = "Cancel";
    private const string INTERACT = "Interact";
    private const string TURNLEFT = "TurnLeft";
    private const string TURNRIGHT = "TurnRight";

    public float x_AxisMouse;
    public float y_AxisMouse;
    public float v_Axis;
    public float h_Axis;
    public bool mouse_0;
    public bool mouse_0_down;
    public bool mouse_0_up;
    public bool l_Shift;
    public bool jump;
    public bool interact;
    public bool cancel;
    public bool buildMenu;
    public bool statsMenu;
    public bool tool_0;
    public bool tool_1;
    public bool tool_2;
    public bool tool_3;
    public bool turnLeft;
    public bool turnRight;
    public bool flashlight;

    public PlayerController playerController;
    public TerrainManager terrainManager;
    

    // Update is called once per frame
    void Update () {
        #region CheckingInput
        x_AxisMouse = Input.GetAxis(MOUSE_X);
        y_AxisMouse = Input.GetAxis(MOUSE_Y);

        v_Axis = Input.GetAxis(V_AXIS);
        h_Axis = Input.GetAxis(H_AXIS);

        l_Shift = Input.GetKey(KeyCode.LeftShift);
        jump = Input.GetButton(JUMP);

        mouse_0 = Input.GetButton(MOUSE_0);
        mouse_0_down = Input.GetButtonDown(MOUSE_0);
        mouse_0_up = Input.GetButtonUp(MOUSE_0);

        turnLeft = Input.GetButtonDown(TURNLEFT);
        turnRight = Input.GetButtonDown(TURNRIGHT);

        interact = Input.GetButtonDown(INTERACT);
        cancel = Input.GetButtonDown(CANCEL);
        buildMenu = Input.GetKeyDown(KeyCode.F2);
        statsMenu = Input.GetKeyDown(KeyCode.F3);
        flashlight = Input.GetKeyDown(KeyCode.L);

        tool_0 = Input.GetKeyDown(KeyCode.Alpha1);
        tool_1 = Input.GetKeyDown(KeyCode.Alpha2);
        tool_2 = Input.GetKeyDown(KeyCode.Alpha3);
        tool_3 = Input.GetKeyDown(KeyCode.Alpha4);
        #endregion

        if (tool_0) playerController.ChangeTool(0);
        if (tool_1) playerController.ChangeTool(1);
        if (tool_2) playerController.ChangeTool(2);
        if (tool_3) playerController.ChangeTool(3);
        if (buildMenu) MainUIManager.Instance.ToggleBuildMenu();
        if (statsMenu) MainUIManager.Instance.ToggleStatsMenu();
        if (cancel) MainUIManager.Instance.TogglePauseScreen();


        if (Input.GetKeyDown(KeyCode.O)) ResourceManager.Instance.CreateResourceBox(ResourceTypes.BioPlastic, playerController.transform.position, transform.rotation);
        if (Input.GetKeyDown(KeyCode.P)) ResourceManager.Instance.CreateResourceBox(ResourceTypes.Metal, playerController.transform.position, transform.rotation);
        if (Input.GetKeyDown(KeyCode.Keypad4)) ResourceManager.Instance.CreateResourceBox(ResourceTypes.MetalOre, playerController.transform.position, transform.rotation);
        if (Input.GetKeyDown(KeyCode.Keypad5)) ResourceManager.Instance.CreateResourceBox(ResourceTypes.RawFood, playerController.transform.position, transform.rotation);
        if (Input.GetKeyDown(KeyCode.F1)) terrainManager.GenerateNewTerrain();
        if (Input.GetKeyDown(KeyCode.Keypad1)) BaseManager.Instance.DisableAllLights();
        if (Input.GetKeyDown(KeyCode.Keypad2)) BaseManager.Instance.EnableAllLights();
    }
}
