using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEntitySoundInstance : MonoBehaviour
{
    public AudioSource m_audioSource;
    public float m_duration;
    public float m_delay;
    public float m_removeTime;
    public bool m_destroyAfterwards;
    public float m_delayRdy;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_delayRdy <= Time.time)
        {
            if (m_duration <= 0)
                m_removeTime = (m_audioSource.loop == true) ? float.MaxValue * 0.5f : m_audioSource.clip.length + Time.time;
            else
                m_removeTime = m_duration + Time.time;

            m_delayRdy = float.MaxValue;
            m_audioSource.Play();
        }

        if (m_removeTime <= Time.time)
            removeSound();
    }

    public void removeSound()
    {
        m_audioSource.Stop();
        Destroy(m_audioSource);
        Destroy(this);

        if(m_destroyAfterwards)
        {
            if (GetComponent<CubeEntitySystem>() == null)
            {
                Destroy(this.gameObject);
            }
            else
                Debug.Log("Aborted: Tried to delete sound gameObject, but it had a CubeEntitySystem script attached to it!");
        }
    }

    public void addSoundToObject(AudioSource source, float duration, bool destroyAfterwards, float delay)
    {
        m_duration = duration;
        m_destroyAfterwards = destroyAfterwards;
        m_delay = delay;
        m_delayRdy = m_delay + Time.time;


        //if (m_duration <= 0)
        //m_duration = source.clip.length;
        //m_finishTime = m_duration + Time.time;
        createAudioSource(source);
    }

    void createAudioSource(AudioSource original)
    {
        m_audioSource = gameObject.AddComponent<AudioSource>();

        m_audioSource.clip                          = original.clip;
        m_audioSource.outputAudioMixerGroup         = original.outputAudioMixerGroup;
        m_audioSource.mute                          = original.mute;
        m_audioSource.bypassEffects                 = original.bypassEffects;
        m_audioSource.bypassListenerEffects         = original.bypassListenerEffects;
        m_audioSource.bypassReverbZones             = original.bypassReverbZones;
        m_audioSource.playOnAwake                   = false;//m_audioSource.playOnAwake = original.playOnAwake;
        m_audioSource.loop                          = original.loop;
        m_audioSource.priority                      = original.priority;
        m_audioSource.volume                        = original.volume;
        m_audioSource.pitch                         = original.pitch;
        m_audioSource.panStereo                     = original.panStereo;
        m_audioSource.spatialBlend                  = original.spatialBlend;
        m_audioSource.reverbZoneMix                 = original.reverbZoneMix;
        m_audioSource.dopplerLevel                  = original.dopplerLevel;
        m_audioSource.spread                        = original.spread;
        m_audioSource.minDistance                   = original.minDistance;
        m_audioSource.maxDistance                   = original.maxDistance;
        m_audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, original.GetCustomCurve(AudioSourceCurveType.CustomRolloff));
        m_audioSource.rolloffMode = AudioRolloffMode.Custom;
    }
}
