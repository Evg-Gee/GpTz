using UnityEngine;

public class GameManager : MonoBehaviour {

	[SerializeField] private CameraController _cameraController;
    [SerializeField] private Transform _cameraTarget; // Объект вокруг которого вращается камера
    
    private void Awake() 
    {
        if (_cameraController != null && _cameraTarget != null)
        {
            _cameraController.Initialize(_cameraTarget);
        }
    }
}
