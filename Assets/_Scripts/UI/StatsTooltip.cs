using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatsTooltip : MonoBehaviour
{
    public string statsName;
    public string statsDescription;

    public void ShowTooltip()
    {
        Invoke("SetTooltipData", 0.25f);
    }

    public void HideTooltip()
    {
        CancelInvoke("SetTooltipData");
        UITooltip.Hide();
    }

    void SetTooltipData()
    {
        UITooltip.AddTitle(statsName);

        UITooltip.AddDescription(statsDescription);

        UITooltip.AnchorToRect(this.transform as RectTransform);
        UITooltip.Show();
    }
}
