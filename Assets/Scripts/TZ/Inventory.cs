using UnityEngine;

// Реализация инвентаря для гаек
public class Inventory : MonoBehaviour, IInventory
{
    public int BoltsCount { get; private set; }

    public void AddBolt()
    {
        BoltsCount++;
        Debug.Log("Гайка добавлена в инвентарь. Всего: " + BoltsCount);
        
        var statusMessageUI = GameObject.FindObjectOfType<StatusMessageService>();
        if (statusMessageUI != null)
        {
            statusMessageUI.ShowMessage("Гайка в инвентаре");
        }
    }

    public bool UseBolt()
    {
        if (BoltsCount <= 0) return false;
        BoltsCount--;
        Debug.Log("Гайка использована. Осталось: " + BoltsCount);
        
        var statusMessageUI = GameObject.FindObjectOfType<StatusMessageService>();
        if (statusMessageUI != null)
        {
            statusMessageUI.ShowMessage("Гайка закручена");
        }
        return true;
    }
}