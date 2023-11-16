using UnityEngine;

public class EnemyEmitterPatternBehaviour : MonoBehaviour
{
    [SerializeField] private float sinPeriod = 5f;
    [SerializeField] private float timerDuration = 5;

    public float progress;
    private float timer;

    private const float minusPi = -Mathf.PI / 2;
    private const float plusPi = Mathf.PI / 2;

    void Start()
    {

    }

    void FixedUpdate()
    {
        if (timer < timerDuration)
        {
            SmoothProgress();
            timer += Time.deltaTime;
            //Use value HERE
        }
        else
        {
            timer = 0;
        }
    }

    private void SmoothProgress()
    {
        progress = timer / sinPeriod;
        progress = Mathf.Lerp(minusPi, plusPi, progress);
        progress = (Mathf.Sin(progress) / 2f) * 0.5f;
    }
}
