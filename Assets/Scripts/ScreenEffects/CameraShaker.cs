using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] float shakeIntensity;
    [SerializeField] float shakeTime;
    [SerializeField] AnimationCurve shakeCurve;

    private float timer;
    private CinemachineBasicMultiChannelPerlin basicNoise;

    private void Awake()
    {
        StopShake();

        DamageEventsManager.OnPlayerDamaged += ShakeCameraOnDamaged;
    }

    private void OnDestroy()
    {
        StopShake();
        DamageEventsManager.OnPlayerDamaged -= ShakeCameraOnDamaged;
    }

    public void ShakeCameraOnDamaged(float damagePercentage)
    {
        basicNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        shakeIntensity = shakeCurve.Evaluate(damagePercentage);

        basicNoise.m_AmplitudeGain = shakeIntensity;

        timer = shakeTime;
    }

    void StopShake()
    {
        if(virtualCamera)
        {
            basicNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            basicNoise.m_AmplitudeGain = 0;
        }
        timer = 0;
    }


    private void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                StopShake();
            }
        }
    }
}
