using UnityEngine;
using System.Linq;

public class SpaceBulletEntity : MonoBehaviour {
    [SerializeField] private float speed = 15;
    [SerializeField] private int damage = 10;

    private void FixedUpdate() {
        transform.Translate(Vector3.up * (speed * Time.fixedDeltaTime));
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) return;
        if (other.CompareTag("Enemy")) {
            var enemy = other.GetComponent<SpaceEnemy>();
            if (enemy != null)
                enemy.GainDamage(damage);
        }

        string[] goThroughObjects = new[] {
            "BlasterFriendly", "Player",
            "ActivatedShield"
        };
        if (goThroughObjects.Contains(other.tag)) {
            //do nothing
        }
        else {
            Destroy(gameObject);
        }
       
    }
    
    
}