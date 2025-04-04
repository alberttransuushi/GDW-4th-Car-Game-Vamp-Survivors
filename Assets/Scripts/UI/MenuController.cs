using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuController : MonoBehaviour
{
    public List<KeyCode> _increaseHoriz;
    public List<KeyCode> _decreaseHoriz;
    public List<KeyCode> _increaseVert;
    public List<KeyCode> _decreaseVert;
    public List<KeyCode> _confirmButtons;
    public List<KeyCode> _cancelButtons;

    private int _activeButton = 0;
    public MenuDefinition _menuDefinition;

    public InputActionReference upControl;
    public InputActionReference downControl;
    public InputActionReference leftControl;
    public InputActionReference rightControl;
    public InputActionReference selectControl;
    public InputActionReference returnControl;

    public void OnEnable()
    {
        upControl.action.Enable();
        downControl.action.Enable();
        leftControl.action.Enable();
        rightControl.action.Enable();
        selectControl.action.Enable();
        returnControl.action.Enable();
    }

    public void OnDisable()
    {
        upControl.action.Disable(); 
        downControl.action.Disable();
        leftControl.action.Disable();
        rightControl.action.Disable();
        selectControl.action.Disable();
        returnControl.action.Disable();
    }

    public void Update()
    {
        switch (_menuDefinition.GetMenuType())
        {
            case MenuType.HORIZONTAL:

                MenuInput(_increaseHoriz, _decreaseHoriz);

                break;
            case MenuType.VERTICAL:

                MenuInput(_increaseVert, _decreaseVert);
                break;
        }
    }

    private void MenuInput(List<KeyCode> increase, List<KeyCode> decrease)
    {
        int newActive = _activeButton;

        for (int i = 0; i < increase.Count; i++)
        {
            if (Input.GetKeyDown(increase[i]) || rightControl.action.WasPressedThisFrame() || downControl.action.WasPressedThisFrame())
            {
                newActive = SwitchCuttentButton(1);
            }
        }
        for(int i = 0; i < decrease.Count; i++)
        {
            if (Input.GetKeyDown(decrease[i]) || leftControl.action.WasPressedThisFrame() || upControl.action.WasPressedThisFrame())
            {
                newActive = SwitchCuttentButton(-1);
            }
        }
        for(int i = 0; i < _confirmButtons.Count; i++)
        {
            if (Input.GetKeyDown(_confirmButtons[i]) || selectControl.action.WasPressedThisFrame())
            {
                ClickCurrentButton();
            }
        }

        _activeButton = newActive;
    }

    private void ClickCurrentButton()
    {
        if (!_menuDefinition._carButtons[_activeButton].GetDisableControls())
        {
            _menuDefinition._carButtons[_activeButton].ClickButton();
        }
    }

    private int SwitchCuttentButton(int increment)
    {

        if (!_menuDefinition._carButtons[_activeButton].GetDisableControls())
        {
            int newActive = Utility.WrapAround(_menuDefinition._carButtons.Count, _activeButton, increment);

            _menuDefinition._carButtons[_activeButton].SwappedOff();
            _menuDefinition._carButtons[newActive].SwappedTo();

            return newActive;
        }
        return _activeButton;
    }
}
