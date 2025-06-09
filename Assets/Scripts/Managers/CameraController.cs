using UnityEngine;

public class CameraController : MonoBehaviour 
{
	[Header("Rotation Settings")]
    [SerializeField] private float _rotationSpeed = 50f;
    [SerializeField] private bool _invertY = false;
    [SerializeField] private bool _invertX = false;
    
    private Transform _target;
    private float _distance;
    private Quaternion _currentRotation;
    private Quaternion _horizontalRotation;
    private Quaternion _verticalRotation;
    private float _currentHorizontalAngle;
    private float _currentVerticalAngle;

    public void Initialize(Transform target)
    {
        _target = target;
        _distance = Vector3.Distance(transform.position, _target.position);
        
        // Вычисляем начальные углы
        Vector3 direction = (_target.position - transform.position).normalized;
        _currentHorizontalAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        _currentVerticalAngle = Mathf.Asin(-direction.y) * Mathf.Rad2Deg;
        
        // Инициализируем вращения
        _horizontalRotation = Quaternion.AngleAxis(_currentHorizontalAngle, Vector3.up);
        _verticalRotation = Quaternion.AngleAxis(_currentVerticalAngle, Vector3.right);
        _currentRotation = _horizontalRotation * _verticalRotation;
        
        // Применяем начальное вращение
        transform.rotation = _currentRotation;
    }

    private void LateUpdate()
    {
        if (_target == null) return;
        
        HandleRotationInput();
        UpdateCameraPosition();
    }

    private void HandleRotationInput()
    {
        // Получаем ввод с клавиатуры
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        // Применяем инверсию
        horizontalInput = _invertX ? -horizontalInput : horizontalInput;
        verticalInput = _invertY ? -verticalInput : verticalInput;
        
        // Обновляем углы
        _currentHorizontalAngle += horizontalInput * _rotationSpeed * Time.deltaTime;
        _currentVerticalAngle += verticalInput * _rotationSpeed * Time.deltaTime;
        
        // Ограничиваем вертикальный угол для предотвращения переворота
        _currentVerticalAngle = Mathf.Clamp(_currentVerticalAngle, -89f, 89f);
        
        // Обновляем кватернионы вращения
        _horizontalRotation = Quaternion.AngleAxis(_currentHorizontalAngle, Vector3.up);
        _verticalRotation = Quaternion.AngleAxis(_currentVerticalAngle, Vector3.right);
        _currentRotation = _horizontalRotation * _verticalRotation;
    }

    private void UpdateCameraPosition()
    {
        // Вычисляем новую позицию камеры
        Vector3 newPosition = _target.position + _currentRotation * Vector3.back * _distance;
        
        // Плавно перемещаем камеру
        transform.position = Vector3.Lerp(
            transform.position, 
            newPosition, 
            Time.deltaTime * 10f
        );
        
        // Камера всегда смотрит на цель
        transform.LookAt(_target.position);
    }
}