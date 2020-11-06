using UnityEngine;

public class SpacePlayer : MonoBehaviour
{
    public int health = 100;
    [Space] [SerializeField] private float speed = 5;

    [SerializeField] private int angleOffset;
    private Camera _camera;
    [SerializeField] private float rotationSpeed = 9;

    private Transform _cameraTransform;
    private float _cameraStartZ;
    [SerializeField] private float cameraSmoothness = 1;
    [SerializeField] private float cameraOffsetDivider = 100;

    [Space] [SerializeField] private float fireRate = 0.5f;
    private float _shootingTimer = 0;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform gunPoint;

    private void Start()
    {
        _camera = Camera.main;
        if (_camera != null)
        {
            _cameraTransform = _camera.transform;
            _cameraStartZ = _cameraTransform.position.z;
        }
    }

    private void FixedUpdate()
    {
        MovementProcessing();
        CameraProcessing();
    }

    private void Update()
    {
        if (_shootingTimer > 0)
            _shootingTimer -= Time.deltaTime;
        else if (Input.GetButton("Fire1"))
        {
            _shootingTimer = fireRate;
            Instantiate(bulletPrefab, gunPoint.position, transform.rotation);
        }
    }

    private void MovementProcessing()
    {
        var horizontalAxis = Input.GetAxis("Horizontal");
        var verticalAxis = Input.GetAxis("Vertical");
        transform.Translate(horizontalAxis * speed * Time.fixedDeltaTime,
            verticalAxis * speed * Time.fixedDeltaTime, 0,
            Space.World);
    }

    private void CameraProcessing()
    {
        if (!_camera) return;

        var position = transform.position;
        var dir = Input.mousePosition - _camera.WorldToScreenPoint(position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + angleOffset;
        var targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        var smooth = rotationSpeed * Time.fixedDeltaTime;
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, smooth);

        position.z = _cameraStartZ;
        position += dir / cameraOffsetDivider;
        _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, position, cameraSmoothness);
    }

    public void GainDamage(int amount)
    {
        if (amount <= 0) return;
        health -= amount;
        if (health <= 0)
        {
            // Death
        }
        else
        {
            // Damage
        }
    }
}