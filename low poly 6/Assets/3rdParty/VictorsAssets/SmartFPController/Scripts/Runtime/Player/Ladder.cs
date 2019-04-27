﻿/********************************************
 * Copyright(c): 2018 Victor Klepikov       *
 *                                          *
 * Profile: 	 http://u3d.as/5Fb		    *
 * Support:      http://smart-assets.org    *
 ********************************************/


using UnityEngine;

namespace SmartFPController
{
    [RequireComponent( typeof( BoxCollider ) )]
    public class Ladder : MonoBehaviour
    {
        public AudioClip[] footstepSounds = null;
        public Transform m_Transform { get; private set; }

        private AudioSource m_Audio = null;
        private AudioClip lastClip = null;


        // Awake
        void Awake()
        {
            m_Transform = transform;

            Collider tmpCollider = GetComponent<Collider>();
            tmpCollider.enabled = true;
            tmpCollider.isTrigger = true;
        }

        // Assign AudioSource
        public void AssignAudioSource( AudioSource audioSource )
        {
            m_Audio = audioSource;

            if( m_Audio != null )
                m_Audio.outputAudioMixerGroup = GameSettings.SFXOutput;
        }


        // PlayLadder FootstepSound
        public void PlayLadderFootstepSound()
        {
            if( m_Audio == null )
                return;

            m_Audio.pitch = Time.timeScale;

            int index = Random.Range( 1, footstepSounds.Length );
            lastClip = footstepSounds[ index ];
            m_Audio.PlayOneShot( lastClip );
            footstepSounds[ index ] = footstepSounds[ 0 ];
            footstepSounds[ 0 ] = lastClip;
        }
    };
}