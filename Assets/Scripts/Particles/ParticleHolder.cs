using DG.Tweening;
using UnityEngine;

namespace Particles
{
    public class ParticleHolder : MonoBehaviour
    {
        [SerializeField] private ParticleSystem enemyDisappearParticle;

        public void SpawnDisappearParticle(Vector3 position)
        {
            var particle = Instantiate(enemyDisappearParticle, position, Quaternion.identity, transform);
            particle.Play();
            
            Destroy(particle.gameObject, 1f);
        }
    }
}