using UnityEngine;
using UnityEngine.AI;

public class SpaceEnemy : MonoBehaviour
{
    public SpacePlayer player;
    public int health = 50;
    public int enemyValue = 100;

    protected Transform PlayerTransform;
    protected NavMeshAgent NavAgent;
    private Vector3 _prevPos;
    [Space] [SerializeField] private float rotationSpeed = 5;

    protected void AIInitialization()
    {
        NavAgent = GetComponent<NavMeshAgent>();
        NavAgent.updateRotation = false;
        NavAgent.updateUpAxis = false;
        PlayerTransform = player.transform;
        _prevPos = PlayerTransform.position;
        NavAgent.destination = _prevPos;
    }

    protected void AIProcessing()
    {
        if (!NavAgent.enabled) return;
        
        var pos = PlayerTransform.position;

        var corners = NavAgent.path.corners;
        if (corners.Length >= 2)
        {
            var position = transform.position;
            Debug.DrawLine(position, corners[1], Color.red);
            var dir = corners[1] - position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
            var targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            var smooth = rotationSpeed * Time.fixedDeltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, smooth);
        }

        if (pos == _prevPos && NavAgent.destination != new Vector3()) return;
        _prevPos = pos;
        NavAgent.destination = pos;
    }

    public void GainDamage(int amount)
    {
        if (amount <= 0) return;
        health -= amount;
        if (health <= 0)
        {
            // Death
            Destroy(gameObject);
        }
        else
        {
            // Just damage
        }
    }
}