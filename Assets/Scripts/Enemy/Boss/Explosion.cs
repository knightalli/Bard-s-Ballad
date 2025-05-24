using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Header("Explosion Settings")]
    [SerializeField] private float lifetime = 1f;           

    [Header("VFX")]
    [SerializeField] private ParticleSystem particles;        

    private void Start()
    {        
        if (particles != null)
            particles.Play();

        Destroy(gameObject, lifetime);
    }
}
