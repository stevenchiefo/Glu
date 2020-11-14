using System.Collections;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI m_TimerText;


    public float TargetTimerInMinutes;
    public float TargetTimerInSeconds;


    public int Miliseconds;
    public int Seconds;
    public int Minutes;





    private IEnumerator CheckTimer()
    {
        while (true)
        {

            Miliseconds++;
            if (Miliseconds >= 60)
            {
                Miliseconds = 0;
                Seconds++;
                if (Seconds >= 60)
                {
                    Seconds = 0;
                    Minutes++;
                }
            }
            m_TimerText.text = $"{Minutes}:{Seconds}:{Miliseconds}";
            yield return new WaitForSeconds(0.01f);


        }
    }

    public void Stop()
    {
        StopAllCoroutines();
    }

    public void StartTimer()
    {

        StartCoroutine(CheckTimer());
    }
}
