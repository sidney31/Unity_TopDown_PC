using UnityEngine;

public class BackgroundSoundPlayer : MonoBehaviour
{
    private void Start()
    {
        FindObjectOfType<AudioManager>().Play("Background");
    }
}
