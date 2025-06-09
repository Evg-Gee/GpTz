using UnityEngine;

// Сервис абстрагирования ввода
public class InputService : MonoBehaviour
{
    //public Ray PointerRay => Camera.main.ScreenPointToRay(Input.mousePosition);
    public Ray PointerRay 
    { 
        get 
        {
            if (Camera.main == null)
            {
                Debug.LogError("MainCamera не найдена!");
                return new Ray(Vector3.zero, Vector3.forward);
            }
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
    public bool WasLeftClick ()
    { 
         
        return Input.GetMouseButtonDown(0); 
       
    }
}