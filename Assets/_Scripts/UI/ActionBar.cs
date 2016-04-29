using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ActionBar : MonoBehaviour
{
    public static List<RuneAssignSkillSlot> skillSlots = new List<RuneAssignSkillSlot>();

    public static void RefreshActionBar()
    {
        foreach(var slot in skillSlots)
        {
            slot.AssignRune();
        }
    }

}
