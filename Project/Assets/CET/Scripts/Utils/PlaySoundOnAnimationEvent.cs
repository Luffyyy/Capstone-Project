using UnityEngine;

public class PlaySoundOnAnimationEvent : MonoBehaviour
{
    public AudioSource source;
    public AudioClip clip;

    public void PlayAnimationSound() {
        if (clip == null)
        {
            source.Play();
        } else
        {
            source.PlayOneShot(clip);
        }
    }
}
