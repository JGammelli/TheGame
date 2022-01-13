using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleHandler : MonoBehaviour
{
    public GameObject slashEffect;


    public void PlayEffect(GameObject effect, Vector2 position, float angle = 0)
    {
        effect.transform.position = position;
        effect.GetComponent<ParticleSystem>().Play();
    }
    
}
