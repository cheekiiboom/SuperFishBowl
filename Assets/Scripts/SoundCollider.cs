using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCollider : MonoBehaviour
{
    [SerializeField]
    private AudioSource m_AudioSource;
    [SerializeField]
    private AudioClip m_Clip;

    // Start is called before the first frame update
    void Start()
    {
        m_Clip = GetComponent<AudioClip>();
        m_AudioSource = GetComponent<AudioSource>();

        m_AudioSource.clip = m_Clip;
    }

    // when something else collides with this object, play the sound
    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.tag.CompareTo("Player") == 0)
            m_AudioSource.Play();
    }
}
