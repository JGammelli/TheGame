using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleHandler : MonoBehaviour
{
    public GameObject slashEffect;
    public GameObject dashOnGroundEffect;


    public void PlayEffect(GameObject effect, Vector2 position, Vector3 rotation)
    {
        effect.transform.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
        effect.transform.position = position;
        effect.GetComponent<ParticleSystem>().Play();
    }

    public void StopEffect(GameObject effect)
    {
        if (effect.GetComponent<ParticleSystem>().isPlaying)
        {
            effect.GetComponent<ParticleSystem>().Stop();
        }
    }
    
}
