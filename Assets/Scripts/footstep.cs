using UnityEngine;
using System.Collections;


public class footstep: MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private float footstepInterval = 0.5f; // 脚步声间隔时间
    [SerializeField] private float loopDuration = 1f; // 控制音频循环的持续时间
    private float lastStepTime;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // 设置AudioSource的loop参数
        audioSource.loop = true;
    }

    void Update()
    {
        // 检查是否按下A或D键，并确保与上一次播放有足够的时间间隔
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && 
            Time.time - lastStepTime >= footstepInterval)
        {
            StartCoroutine(PlayFootstepSound());
            lastStepTime = Time.time;
        }
    }

    private IEnumerator PlayFootstepSound()
    {
        audioSource.Play();
        yield return new WaitForSeconds(loopDuration);
        audioSource.Stop();
    }
}