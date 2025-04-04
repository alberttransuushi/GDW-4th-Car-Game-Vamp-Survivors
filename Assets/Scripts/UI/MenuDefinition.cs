using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MenuType
{
    HORIZONTAL,
    VERTICAL
}
public class MenuDefinition : MonoBehaviour
{


    public List<ButtonBehaviour> _carButtons = new List<ButtonBehaviour>();
    public MenuType menuType = MenuType.HORIZONTAL;

    public MenuType GetMenuType()
    {
        return menuType;
    }


}
    
