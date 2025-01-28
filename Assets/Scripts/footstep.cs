using UnityEngine;
using System.Collections;

public class footstep: MonoBehaviour
{
    private AudioSource footstepAudioSource;
    private AudioSource jumpAudioSource;
    [SerializeField] private float footstepInterval = 0.5f;
    [SerializeField] private float loopDuration = 1f;
    private float lastStepTime;
    private Animator animator; // Reference to the animator

    void Start()
    {
        // Get both audio sources
        AudioSource[] audioSources = GetComponents<AudioSource>();
        footstepAudioSource = audioSources[0]; // First audio source for footsteps

        footstepAudioSource.loop = true;
        jumpAudioSource.loop = false; // Make sure jump sound doesn't loop
        
        animator = GetComponent<Animator>(); // Get the animator component
    }

    void Update()
    {
        // Existing footstep logic
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && 
            Time.time - lastStepTime >= footstepInterval)
        {
            StartCoroutine(PlayFootstepSound());
            lastStepTime = Time.time;
        }

    }

    private IEnumerator PlayFootstepSound()
    {
        footstepAudioSource.Play();
        yield return new WaitForSeconds(loopDuration);
        footstepAudioSource.Stop();
    }

    private void PlayJumpSound()
    {
        if (!jumpAudioSource.isPlaying)
        {
            jumpAudioSource.Play();
        }
    }
}