using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public List<KeyCode> _increaseHoriz;
    public List<KeyCode> _decreaseHoriz;
    public List<KeyCode> _confirmButtons;
    public List<KeyCode> _cancelButtons;

    private int _activeButton = 0;
    public MenuDefinition _menuDefinition;

    public void Update()
    {
        MenuInput(_increaseHoriz, _decreaseHoriz);
    }

    private void MenuInput(List<KeyCode> increase, List<KeyCode> decrease)
    {
        int newActive = _activeButton;

        for (int i = 0; i < increase.Count; i++)
        {
            if (Input.GetKeyDown(increase[i]))
            {
                newActive = SwitchCuttentButton(1);
            }
        }
        for(int i = 0; i < decrease.Count; i++)
        {
            if (Input.GetKeyDown(decrease[i]))
            {
                newActive = SwitchCuttentButton(-1);
            }
        }
        for(int i = 0; i < _confirmButtons.Count; i++)
        {
            if (Input.GetKeyDown(_confirmButtons[i]))
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
