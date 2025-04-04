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

    [SerializeField]
    public int _activeButton = 0;
    public MenuDefinition _menuDefinition;

    public InputActionReference upControl;
    public InputActionReference downControl;
    public InputActionReference leftControl;
    public InputActionReference rightControl;
    public InputActionReference selectControl;
    public InputActionReference returnControl;

    [SerializeField]
    MenuDefinition WeaponSelectMenu;

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

                MenuInput(_increaseHoriz, _decreaseHoriz, rightControl, leftControl);

                break;
            case MenuType.VERTICAL:

                MenuInput(_increaseVert, _decreaseVert, downControl, upControl);
                break;
        }
    }

    private void MenuInput(List<KeyCode> increase, List<KeyCode> decrease, InputActionReference rightOrDown, InputActionReference leftOrUp)
    {
        int newActive = _activeButton;
        //Debug.Log(_menuDefinition._carButtons[1]);

        for (int i = 0; i < increase.Count; i++)
        {
            if (Input.GetKeyDown(increase[i]) || rightOrDown.action.WasPressedThisFrame())
            {
                //Debug.Log("PressedRight");
                newActive = SwitchCuttentButton(1);

            }
        }
        for(int i = 0; i < decrease.Count; i++)
        {
            if (Input.GetKeyDown(decrease[i]) || leftOrUp.action.WasPressedThisFrame())
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
        //Debug.Log("Checking Disable Controls...");
        if (!_menuDefinition._carButtons[_activeButton].GetDisableControls())
        {

            int newActive = Utility.WrapAround(_menuDefinition._carButtons.Count, _activeButton, increment);
            //Debug.Log("Swapping from " + _activeButton + " to " + newActive);
            _menuDefinition._carButtons[_activeButton].SwappedOff();
            _menuDefinition._carButtons[newActive].SwappedTo();

            return newActive;
        }
        return _activeButton;
    }

    public void changeActiveMenu()
    {
        _menuDefinition.gameObject.SetActive(false);
        WeaponSelectMenu.gameObject.SetActive(true);
        _menuDefinition = WeaponSelectMenu.gameObject.GetComponent<MenuDefinition>();
    }
}
