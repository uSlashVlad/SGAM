using UnityEngine;
using UnityEngine.AI;

public class SpaceEnemy : MonoBehaviour
{
    public SpacePlayer player;
    public int health = 50;
    public int enemyValue = 100;

    protected Transform PlayerTransform;
    private Vector3 _prevPos;
    [Space] [SerializeField] private float rotationSpeed = 5;
    

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