using UnityEngine;
using UnityEngine.UI; //for accessing Sliders and Dropdown
using UnityEngine.Audio;
using System.Collections.Generic; // So we can use List<>
using System.Diagnostics;
using System;

//using Pitch;

[RequireComponent(typeof(AudioSource))]
public class MicrophoneInput : MonoBehaviour
{
    //public float minThreshold = 0;
    //public float frequency = 0.0f;
    public int audioSampleRate = 44100;
    //public string microphone;
    //public FFTWindow fftWindow;
    //public Dropdown micDropdown;
    //public Slider thresholdSlider;

    //private List<string> options = new List<string>();
    //private int samples = 8192;
    //private AudioSource audioSource;

    private string mic;

    public float loudness = 0.0f;
    public float force = 0.0f;
    //public float waves = 0.1f;
    //public float threshold = 0.0f;
    private AudioSource audioSource;

    public AudioMixerGroup audioMixMic;

    //private PitchTracker pitchTracker;
    //public PitchTracker.PitchRecord pitch;


    private float timer = 0.0f;
    private float waitTime = 5.0f;
    private float accu = 0.0f;
    private int n = 0;



    public bool run;

    public float RmsValue;
    public float DbValue;
    public float PitchValue;

    private const int QSamples = 512;
    private const float RefValue = 0.1f;
    private float Threshold = 0.003f;

    float[] _samples;
    private float[] _spectrum;
    private float _fSample;

    private int nT;

    private void Start()
    {

        foreach (var device in Microphone.devices)
        {
            UnityEngine.Debug.Log("Name: " + device);
        }
        mic = Microphone.devices[0];

        audioSource = GetComponent<AudioSource>();
        //pitchTracker = new PitchTracker();
        //pitchTracker.SampleRate = 44100.0;


        audioSource.clip = Microphone.Start(mic, true, 10, 44100);
        audioSource.outputAudioMixerGroup = audioMixMic;
        audioSource.loop = true;
        while (!(Microphone.GetPosition(null) > 0)) { }
        audioSource.Play();


        //https://answers.unity.com/questions/157940/getoutputdata-and-getspectrumdata-they-represent-t.html
        _samples = new float[QSamples];
        _spectrum = new float[QSamples];
        _fSample = AudioSettings.outputSampleRate;




    }

    private void Update()
    {



        audioSource.GetOutputData(_samples, 0); // fill array with samples
        int i;
        float sum = 0;

        for (i = 0; i < QSamples; i++)
        {
            sum += _samples[i] * _samples[i]; // sum squared samples
        }

        RmsValue = Mathf.Sqrt(sum / QSamples); // rms = square root of average
        DbValue = 20 * Mathf.Log10(RmsValue / RefValue); // calculate dB
        if (DbValue < -160)
        {
            DbValue = -160; // clamp it to -160dB min

        }
        //get sound spectrum

        audioSource.GetSpectrumData(_spectrum, 0, FFTWindow.BlackmanHarris);
        float maxV = 0;
        var maxN = 0;
        for (i = 0; i < QSamples; i++)
        { // find max
            if (!(_spectrum[i] > maxV) || !(_spectrum[i] > Threshold))
            {
                continue;
            }


            maxV = _spectrum[i];
            maxN = i; // maxN is the index of max
        }
        float freqN = maxN; // pass the index to a float variable
        if (maxN > 0 && maxN < QSamples - 1)
        { // interpolate index using neighbours
            var dL = _spectrum[maxN - 1] / _spectrum[maxN];
            var dR = _spectrum[maxN + 1] / _spectrum[maxN];
            freqN += 0.5f * (dR * dR - dL * dL);
        }
        PitchValue = freqN * (_fSample / 2) / QSamples; // convert index to frequency
                                                        //float fundamentalFrequency = 0.0f;
                                                        //float[] spectrum = new float[256];



        force += maxV* 10 * Time.deltaTime;
        force = force - force * 0.003f;
        force = Mathf.Clamp(force, 0.01f, 0.5f);


        timer += Time.deltaTime;
        if (timer < waitTime)
        {
            accu += maxV;
            n += 1;

            Threshold = accu * 10 / n;
            if (Threshold < 0.003f)
            {
                Threshold = 0.003f;
            }


        }
        UnityEngine.Debug.Log(Threshold);




    }



    //private void OnGUI()
    //{
    //    GUI.Label(new Rect(600, 500, 300, 40), "pitch: " + PitchValue);
    //    GUI.Label(new Rect(700, 500, 300, 40), "force: " + force);
    //}

    //audioSource.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);

    //for (int i = 1; i < spectrum.Length - 1; i++)
    //{
    //    UnityEngine.Debug.DrawLine(new Vector3(i - 1, spectrum[i] + 10, 0), new Vector3(i, spectrum[i + 1] + 10, 0), Color.red);
    //    UnityEngine.Debug.DrawLine(new Vector3(i - 1, Mathf.Log(spectrum[i - 1]) + 10, 2), new Vector3(i, Mathf.Log(spectrum[i]) + 10, 2), Color.cyan);
    //    UnityEngine.Debug.DrawLine(new Vector3(Mathf.Log(i - 1), spectrum[i - 1] - 10, 1), new Vector3(Mathf.Log(i), spectrum[i] - 10, 1), Color.green);
    //    UnityEngine.Debug.DrawLine(new Vector3(Mathf.Log(i - 1), Mathf.Log(spectrum[i - 1]), 3), new Vector3(Mathf.Log(i), Mathf.Log(spectrum[i]), 3), Color.blue);
    //}

    //pitchTracker.ProcessBuffer(spectrum);
    //pitch = pitchTracker.CurrentPitchRecord;
    //Debug.Log(pitch.ToString());

    //float s = 0.0f;
    //int k = 0;
    //for (int j = 1; j < 256; j++)
    //{
    //    if (spectrum[j] > threshold) // volumn must meet minimum threshold
    //    {
    //        if (s < spectrum[j])
    //        {
    //            s = spectrum[j];
    //            k = j;
    //        }
    //    }
    //}

    //if (timer < waitTime)
    //{
    //    if (s > 0)
    //    {
    //        accu += s;
    //        n += 1;
    //    }

    //    if (timer + Time.deltaTime > waitTime)
    //    {
    //        threshold = accu / n;
    //    }

    //}



    //loudness = s*10;
    //force += loudness * Time.deltaTime;
    //force = force - force * 0.003f;
    //waves += loudness * Time.deltaTime * 0.1f;
    //waves = waves - waves * 0.0002f;
    //if (force > 1.0f)
    //{
    //    force = 1.0f;
    //}
    //if (force <= 0.0f)
    //{
    //    force = 0.0f;
    //}
    //if (waves > 1.0f)
    //{
    //    waves = 1.0f;
    //}
    //if (waves <= 0.1f)
    //{
    //    waves = 0.1f;
    //}

    //fundamentalFrequency = k * audioSampleRate / 256;
    //if (fundamentalFrequency != 0) { Debug.Log(fundamentalFrequency); }





    //{


    //    //get components you'll need
    //    audioSource = GetComponent<AudioSource>();

    //    // get all available microphones
    //    foreach (string device in Microphone.devices)
    //    {
    //        if (microphone == null)
    //        {
    //            //set default mic to first mic found.
    //            microphone = device;
    //        }
    //        options.Add(device);
    //    }

    //    minThreshold = 0.2f;

    //    //add mics to dropdown
    //    micDropdown.AddOptions(options);
    //    micDropdown.onValueChanged.AddListener(delegate {
    //        micDropdownValueChangedHandler(micDropdown);
    //    });

    //    thresholdSlider.onValueChanged.AddListener(delegate {
    //        thresholdValueChangedHandler(thresholdSlider);
    //    });
    //    //initialize input with default mic
    //    UpdateMicrophone();
    //}

    //void UpdateMicrophone()
    //{
    //    audioSource.Stop();
    //    //Start recording to audioclip from the mic
    //    audioSource.clip = Microphone.Start(microphone, true, 10, audioSampleRate);
    //    audioSource.loop = true;
    //    // Mute the sound with an Audio Mixer group becuase we don't want the player to hear it
    //    Debug.Log(Microphone.IsRecording(microphone).ToString());

    //    if (Microphone.IsRecording(microphone))
    //    { //check that the mic is recording, otherwise you'll get stuck in an infinite loop waiting for it to start
    //        while (!(Microphone.GetPosition(microphone) > 0))
    //        {
    //        } // Wait until the recording has started.

    //        Debug.Log("recording started with " + microphone);

    //        // Start playing the audio source
    //        audioSource.Play();
    //    }
    //    else
    //    {
    //        //microphone doesn't work for some reason

    //        Debug.Log(microphone + " doesn't work!");
    //    }
    //}


    //public void micDropdownValueChangedHandler(Dropdown mic)
    //{
    //    microphone = options[mic.value];
    //    UpdateMicrophone();
    //}

    //public void thresholdValueChangedHandler(Slider thresholdSlider)
    //{
    //    minThreshold = thresholdSlider.value;
    //}

    //public float GetAveragedVolume()
    //{
    //    float[] data = new float[256];
    //    float a = 0;
    //    audioSource.GetOutputData(data, 0);
    //    foreach (float s in data)
    //    {
    //        a += Mathf.Abs(s);
    //    }
    //    return a / 256;
    //}

    //public float GetFundamentalFrequency()
    //{
    //    float fundamentalFrequency = 0.0f;
    //    int samples = 256;
    //    float[] data = new float[samples];
    //    audioSource.GetSpectrumData(data, 0, FFTWindow.Rectangular);
    //    float s = 0.0f;
    //    int i = 0;
    //    for (int j = 1; j < samples; j++)
    //    {
    //        if (data[j] > minThreshold) // volumn must meet minimum threshold
    //        {
    //            if (s < data[j])
    //            {
    //                s = data[j];
    //                i = j;
    //            }
    //        }
    //    }
    //    fundamentalFrequency = i * audioSampleRate / samples;
    //    frequency = fundamentalFrequency;
    //    return fundamentalFrequency;
    //
    //}
}
