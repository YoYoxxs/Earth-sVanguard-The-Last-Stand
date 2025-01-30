using System;
using UnityEngine;

namespace Shmup
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed;
        [SerializeField] GameObject muzzlePrefab;
        [SerializeField] GameObject hitPrefab;

        Transform parent;

        public void SetSpeed(float speed) => this.speed = speed;
        public void SetParent(Transform parent) => this.parent = parent;

        public Action Callback;

        void Start()
        {
            if (muzzlePrefab != null)
            {
                var muzzleVFX = Instantiate(muzzlePrefab, transform.position, Quaternion.identity);
                muzzleVFX.transform.forward = gameObject.transform.forward;
                muzzleVFX.transform.SetParent(parent);

                DestroyParticleSystem(muzzleVFX);

            }
        }

        void Update()
        {
            transform.SetParent(null);
            transform.position += transform.forward * (speed * Time.deltaTime);

            Callback?.Invoke();
        }

        void OnCollisionEnter(Collision collision)
        {
            // Instantiate hit VFX if needed
            if (hitPrefab != null)
            {
                ContactPoint contact = collision.contacts[0];
                var hitVFX = Instantiate(hitPrefab, contact.point, Quaternion.identity);
                DestroyParticleSystem(hitVFX);
            }

            // Check if the collision object is the player or an enemy (both take damage)
            var player = collision.gameObject.GetComponent<Player>(); // Assuming Player script exists
            if (player != null)
            {
                player.TakeDamage(10); // Apply damage to player
            }

            var enemy = collision.gameObject.GetComponent<Enemy>(); // Assuming Enemy script exists
            if (enemy != null)
            {
                enemy.TakeDamage(10); // Apply damage to enemy
            }

            Destroy(gameObject); // Destroy projectile after collision
        }


        void DestroyParticleSystem(GameObject vfx)
        {
            var ps = vfx.GetComponent<ParticleSystem>();
            if (ps == null)
            {
                ps = vfx.GetComponentInChildren<ParticleSystem>();
            }
            Destroy(vfx, ps.main.duration);
        }
    }
}