using System;
using UnityEngine;

// Интерфейс для инвентаря
public interface IInventory
{
    int BoltsCount { get; }
    void AddBolt();
    bool UseBolt();
}