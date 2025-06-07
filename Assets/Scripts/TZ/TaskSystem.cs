using System;
using System.Collections.Generic;
using UnityEngine;

// Система управления заданиями и их выполнением
public class TaskSystem : MonoBehaviour
{
    [Serializable]
    public class Task
    {
        public string Description;
        public Func<bool> CompletionPredicate;
        public bool IsCompleted;
    }

    [SerializeField] private UnityEngine.UI.Text _taskText;
    private List<Task> _tasks = new List<Task>();

    private void Start()
    {
        // Пример задания: закрыть вентиль
        _tasks.Add(new Task {
            Description = "Перекрыть подачу ливневых вод во вторую емкость",
            CompletionPredicate = () => FindObjectOfType<ValveInteractable>().IsClosed == true,
            IsCompleted = false
        });

        UpdateUI();

        // Подписаться на все события изменения состояния взаимодействуемых объектов
        foreach (var interactable in FindObjectsOfType<MonoBehaviour>())
        {
            // if (interactable is IInteractables ii)
            // ii.OnStateChanged += OnStateChanged;
            MonoBehaviour[] monoBehaviours = FindObjectsOfType<MonoBehaviour>();
    foreach (MonoBehaviour mb in monoBehaviours)
    {
        IInteractables ii = mb as IInteractables;
        if (ii != null)
        {
            ii.OnStateChanged += OnStateChanged;
        }
    }
        }
    }

    private void OnStateChanged(IInteractables obj)
    {
        foreach (var task in _tasks)
        {
            if (!task.IsCompleted && task.CompletionPredicate())
            {
                task.IsCompleted = true;
                ServiceLocator.Resolve<StatusMessageService>().ShowMessage("Задание выполнено: " + task.Description);
            }
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        var next = _tasks.Find(t => !t.IsCompleted);
        if (next != null)
            _taskText.text = next.Description;
        else
            _taskText.text = "Все задания выполнены";
    }
}