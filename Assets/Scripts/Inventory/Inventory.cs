using UnityEngine;

// Реализация инвентаря для гаек
public class Inventory : MonoBehaviour, IInventory
{
    [SerializeField] private UnityEngine.UI.Text boltsCountText;
    public int BoltsCount { get; private set; }

    public void AddBolt()
    {
        BoltsCount++;
        
        var statusMessageUI = GameObject.FindObjectOfType<StatusMessageService>();
        if (statusMessageUI != null)
        {
            statusMessageUI.ShowMessage("Гайка добавлена в инвентарь.");
            boltsCountText.text = BoltsCount.ToString();
        }
    }

    public bool UseBolt()
    {
        if (BoltsCount <= 0) return false;
        BoltsCount--;
        
        var statusMessageUI = GameObject.FindObjectOfType<StatusMessageService>();
        if (statusMessageUI != null)
        {
            statusMessageUI.ShowMessage("Гайка закручена");
            boltsCountText.text = BoltsCount.ToString();
        }
        return true;
    }
}