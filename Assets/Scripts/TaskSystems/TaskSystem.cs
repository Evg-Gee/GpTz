using System.Linq;
using UnityEngine;

public class TaskSystem : MonoBehaviour
{
    [SerializeField] private Valve targetValve;
    [SerializeField] private Bolt[] bolts;
    [SerializeField] private Lid targetLid;
    [SerializeField] private Debris targetDebris;
    [SerializeField] private StatusMessage statusMessageUI;

    private bool valveClosed = false;
    private bool boltsRemoved = false;
    private bool lidRemoved = false;
    private bool debrisCleaned = false;

    private void Update()
    {
        CheckTaskCompletion();
    }

    private void CheckTaskCompletion()
    {
        // Закрыть вентиль
        if (!valveClosed && targetValve.IsClosed)
        {
            valveClosed = true;
            statusMessageUI.ShowHint("Открутите гайки");
        }

        // Открутить гайки
        if (!boltsRemoved && bolts.All(b => b.IsLoose))
        {
            boltsRemoved = true;
            statusMessageUI.ShowHint("Снимите крышку");
        }

        // Снять крышку
        if (!lidRemoved && targetLid.IsOpen)
        {
            lidRemoved = true;
            statusMessageUI.ShowHint("Удалите грязь");
        }

        // Удалить грязь
        if (!debrisCleaned && targetDebris==null)
        {
            debrisCleaned = true;
            statusMessageUI.ShowHint("Завершите задание");
        }
    }
}