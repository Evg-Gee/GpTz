using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskSystem : MonoBehaviour
{
    [Serializable]
    public class Task
    {
        public string Description;
        public Func<bool> CompletionPredicate;
        public bool IsCompleted;
    }

    [SerializeField]
    private Text _taskText;  // Ссылка на UI Text

    private List<Task> _tasks;

    private void Awake()
    {
        _tasks = new List<Task>();
    }

    private void Start()
    {
        ToolManagers toolMgr = ServiceLocator.Resolve<ToolManagers>();

        // 1. Закрыть задвижку подачи ливневых вод в емкость резервуара-накопителя
        var t1 = new Task
        {
            Description = "Закрыть задвижку подачи ливневых вод в емкость резервуара-накопителя",
            CompletionPredicate = delegate ()
            {
                var valve = FindObjectOfType<ValveInteractable>();
                return valve != null && valve.IsClosed;
            },
            IsCompleted = false
        };
        _tasks.Add(t1);

        // 2. Выбрать гаечный ключ S24 на инструментальной панели
        var t2 = new Task
        {
            Description = "Выбрать гаечный ключ S24 и Открутить гайки крышки корпуса обратного клапана ",
            CompletionPredicate = delegate () { return toolMgr.ActiveTool != null && toolMgr.ActiveTool.Type == ToolType.Wrench; },
            IsCompleted = false
        };
        _tasks.Add(t2);

        // 3. Открутить гайки крышки корпуса обратного клапана
        var t3 = new Task
        {
            Description = "Выбрать гаечный ключ S24 и Открутить гайки крышки корпуса обратного клапана ",
            CompletionPredicate = delegate ()
            {
                var bolts = FindObjectsOfType<BoltInteractable>();
                foreach (var b in bolts) if (b.IsTightened) return false;
                return true;
            },
            IsCompleted = false
        };
        _tasks.Add(t3);

        // 4. Снять крышку с корпуса обратного клапана
        var t4 = new Task
        {
            Description = "Снять крышку с корпуса обратного клапана",
            CompletionPredicate = delegate ()
            {
                var lid = FindObjectOfType<LidInteractable>();
                return lid != null && lid.IsOpen && lid.IsInInventory;
            },
            IsCompleted = false
        };
        _tasks.Add(t4);

        // 5. Удалить твердые загрязнения из корпуса обратного клапана
        var t5 = new Task
        {
            Description = "Удалить твердые загрязнения из корпуса обратного клапана",
            CompletionPredicate = delegate () { return FindObjectOfType<DirtInteractable>().IsCleaned == true; },
            IsCompleted = false
        };
        _tasks.Add(t5);

        // 6. Выбрать крышку корпуса обратного клапана на инструментальной панели
        var t6 = new Task
        {
            Description = "Выбрать крышку корпуса обратного клапана на инструментальной панели и Установить крышку на корпус обратного клапана",
            CompletionPredicate = delegate ()
            {
                return toolMgr.ActiveTool != null &&
            toolMgr.ActiveTool.Type == ToolType.Lid;
            },
            IsCompleted = false
        };
        _tasks.Add(t6);

        // 7. Установить крышку на корпус обратного клапана
                var t7 = new Task {
            Description = "Установить крышку на корпус обратного клапана",
            CompletionPredicate = delegate() {
                var lid = FindObjectOfType<LidInteractable>();
                return lid != null && lid.IsInInventory && lid.IsOpen;
            },
            IsCompleted = false
        };
        _tasks.Add(t7);

        // 8. Выбрать гайки на инструментальной панели
        var t8 = new Task
        {
            Description = "Выбрать гайки на инструментальной панели",
            CompletionPredicate = delegate ()
            {
                var bolts = FindObjectsOfType<BoltInteractable>();
                foreach (var b in bolts) if (!b.IsPlaceholderVisible) return false;
                return true;
            },
            IsCompleted = false
        };
        _tasks.Add(t8);

var t9 = new Task
    {
        Description = "Установить гайки на болты",
        CompletionPredicate = delegate()
        {
            BoltInteractable[] boltsArr = FindObjectsOfType<BoltInteractable>();
            for (int i = 0; i < boltsArr.Length; i++)
            {
                if (!boltsArr[i].IsPlacing)
                    return false;   // хотя бы одна гайка ещё не наживлена
            }
            return true; // все гайки наживлены
        },
        IsCompleted = false
    };
    _tasks.Add(t9);

    // 11. Затянуть гайки
    var t11 = new Task
    {
        Description = "Затянуть гайки",
        CompletionPredicate = delegate()
        {
            BoltInteractable[] boltsArr = FindObjectsOfType<BoltInteractable>();
            for (int i = 0; i < boltsArr.Length; i++)
            {
                if (!boltsArr[i].IsTightened && toolMgr.ActiveTool.Type != ToolType.Wrench )
                    return false;   // хотя бы одна гайка ещё не затянута
            }
            return true; // все гайки затянуты
        },
        IsCompleted = false
    };
    _tasks.Add(t11);

    // 12. Открыть задвижку подачи ливневых вод во вторую емкость резервуара
    var t12 = new Task
    {
        Description = "Затянуть гайки и Открыть задвижку подачи ливневых вод во вторую емкость резервуара",
        CompletionPredicate = delegate()
        {
            ValveInteractable valve = FindObjectOfType<ValveInteractable>();
            return valve != null && valve.IsInstalled && !valve.IsClosed;
        },
        IsCompleted = false
    };
    _tasks.Add(t12);
        // Подписка на события всех IInteractable
        MonoBehaviour[] monos = FindObjectsOfType<MonoBehaviour>();
        for (int i = 0; i < monos.Length; i++)
        {
            IInteractables ii = monos[i] as IInteractables;
            if (ii != null)
                ii.OnStateChanged += OnStateChanged;
        }

        UpdateUI();
    }
    // Вызывается при любом изменении состояния IInteractable
    private void OnStateChanged(IInteractables obj)
    {
        EvaluateTasks();
    UpdateUI();
     
    }

    // Обновляем текст текущего задания в UI
    private void UpdateUI()
    {
       for (int i = 0; i < _tasks.Count; i++)
        {
            if (!_tasks[i].IsCompleted)
            {
                _taskText.text = _tasks[i].Description;
                return;
            }
        }
        _taskText.text = "Все задания выполнены";
    }
    private void OnToolChanged(ITool newTool)
{
    // Проверяем задачи в том же формате, что и в OnStateChanged
    EvaluateTasks();
    UpdateUI();
}
    private void EvaluateTasks()
{
    for (int i = 0; i < _tasks.Count; i++)
    {
        if (!_tasks[i].IsCompleted && _tasks[i].CompletionPredicate())
        {
            _tasks[i].IsCompleted = true;
            ServiceLocator.Resolve<StatusMessageService>()
                          .ShowMessage("Задание выполнено: " + _tasks[i].Description);
            break;
        }
    }
}
    
}
