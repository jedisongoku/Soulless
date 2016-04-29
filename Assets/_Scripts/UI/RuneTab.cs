using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RuneTab : MonoBehaviour
{
    public string TabOrMenu;

    public void OnToggle(int id)
    {
        if(GetComponent<Toggle>().isOn)
        {
            if (TabOrMenu == "Tab")
            {
                RuneWindow.activeTab = id;
                RuneWindow.ToggleWindows();
            }
            else
            {
                RuneWindow.activeMenu = id;
                RuneWindow.ToggleWindows();
            }
        }
    }





	
}
