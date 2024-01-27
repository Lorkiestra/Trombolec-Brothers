using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundVisualizer : MonoBehaviour
{
    private AudioSource visSource;
    public Transform[] TRUMPets;
    private float[] targets;
    public int[] freqPointers;
    private float[] oldScale;
    public float changeThreshond;
    public float changeSpeed;
    public float freqMagnitude;
    public float minScale;
    public float maxScale;

    public static float[] rawSamples = new float[512];
    public static float[] frequencies = new float[8];
    // Start is called before the first frame update
    void Start()
    {
        visSource = GetComponent<AudioSource>();
        oldScale = new float[TRUMPets.Length];
        targets = new float[TRUMPets.Length];
    }

    // Update is called once per frame
    void Update()
    {
        GetAudioData();

        //applying data to objects:
        for (int i = 0; i < TRUMPets.Length; i++)
		{
            int ptr = freqPointers[i];
            /*
            if (ptr != i)
			{
                Debug.Log("synch");
			}
            */
            float change = frequencies[ptr] * freqMagnitude;
            if (Mathf.Abs(oldScale[i] - change) >= changeThreshond)
			{
                targets[i] = Mathf.Max(minScale, Mathf.Min(maxScale, change));
                oldScale[i] = change;
            }

            //going for targets
            if (TRUMPets[i].localScale.z < targets[i])
			{
                TRUMPets[i].localScale = new Vector3(TRUMPets[i].localScale.x, TRUMPets[i].localScale.y, targets[i]);
            }
            else
			{
                TRUMPets[i].localScale = new Vector3(TRUMPets[i].localScale.x, TRUMPets[i].localScale.y, Mathf.Max(targets[i], TRUMPets[i].localScale.z - changeSpeed * Time.deltaTime));
            }

        }


    }

    public void GetAudioData()
	{
        //getting original audio samples:
        visSource.GetSpectrumData(rawSamples, 0, FFTWindow.Blackman);

        //getting frequency data:
        int count = 0;
        for (int i = 0; i < 8; i++)
        {
            float avarage = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;
            if (i == 7)
            {
                sampleCount += 2;
            }
            for (int j = 0; j < sampleCount; j++)
            {
                avarage += rawSamples[count] * (count + 1);
                count++;
            }

            avarage /= count;
            frequencies[i] = avarage * 10;
        }
    }
}
