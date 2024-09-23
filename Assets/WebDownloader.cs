using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class WebDownload : MonoBehaviour
{
    [SerializeField] private Clocks clocks;
    private int timeZone = 3;
    private float timer;
    private bool sync;

    void Start()
    {
        StartCoroutine(GetRequest("https://worldtimeapi.org/api/Etc/UTC"));
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer >= 3600f)
        {
            StartCoroutine(GetRequest("https://worldtimeapi.org/api/Etc/UTC"));
            timer = 0f;
        }
    }

    IEnumerator GetRequest(string uri)
    {
        sync = true;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                case UnityWebRequest.Result.ProtocolError:
                    Debug.Log("Error Accured" + webRequest.error); break;
                case UnityWebRequest.Result.Success:
                    ParseTimeStemp(webRequest.downloadHandler.text);
                    webRequest.Dispose();
                    break;
            }
            sync = false;
        }
    }

    public void SynchronizeTime()
    {
        if(!sync)
            StartCoroutine(GetRequest("https://worldtimeapi.org/api/Etc/UTC"));
    }

    private void ParseTimeStemp(string text)
    {
        DateTimeStemp time = JsonUtility.FromJson<DateTimeStemp>(text);
        long timestemp = time.unixtime;

        //For Seconds
        long hours = (timestemp % (60 * 60 * 24) / 60 / 60) + timeZone;
        long minutes = timestemp % (60 * 60) / 60;
        long seconds = timestemp % (60);

        //For Milliseconds
        /*long hours = (timestemp % (1000 * 60 * 60 * 24) / 1000 / 60 / 60) + timeZone;
        long minutes = timestemp % (1000 * 60 * 60) / 1000 / 60;
        long seconds = timestemp % (1000 * 60) / 1000;*/

        ClockTime t;
        t.hours = (int)hours;
        t.minutes = (int)minutes;
        t.seconds = (int)seconds;

        clocks.ReceiveTime(t);
    }

    [System.Serializable]
    public class DateTimeStemp
    {
        public long unixtime;
    }
}
