//
// Klak - Utilities for creative coding with Unity
//
// Copyright (C) 2016 Keijiro Takahashi
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
using UnityEngine;

namespace Klak.Wiring
{
    [AddComponentMenu("Klak/Wiring/Input/PUTAPUTA")]
    public class PUTAPUTA : NodeBase
    {
        #region Editable properties
        [SerializeField]
        AudioSource audioSource;
        #endregion

        #region Node I/O
        [SerializeField, Outlet]
        FloatEvent _lowOutputEvent = new FloatEvent();
        [SerializeField, Outlet]
        FloatEvent _midOutputEvent = new FloatEvent();
        [SerializeField, Outlet]
        FloatEvent _highOutputEvent = new FloatEvent();
        #endregion

        #region MonoBehaviour functions
        private int samples = 256;
        float[] sampleData = new float[256];
        float[] spectrumData = new float[256];
        public FFTWindow windowUsed = FFTWindow.BlackmanHarris;
        float lowValue = 0;
        float midValue = 0;
        float highValue = 0;
        float multiplier = 10;
        void Start()
        {
            audioSource.Play();
        }

        void Update()
        {
            float rms = getSimpleRMS();
            _lowOutputEvent.Invoke(rms * multiplier);
            _midOutputEvent.Invoke(rms * multiplier);
            _highOutputEvent.Invoke(rms * multiplier);
        }

        /*void calculateSimpleSpectrum() {
            float multiplier = 100000;
            audioSource.GetSpectrumData(spectrumData, 0, windowUsed);
            int interval = 150;
            float lowValue = 0;
            for(int i = 150;  i < interval; i++ ) {
                if(spectrumData[i] > lowValue) {
                    lowValue = spectrumData[i];
                }
            }
            //lowValue /= interval;
            this.lowValue = lowValue * multiplier;

            float midValue = 0;
            for(int i = interval;  i < interval*2; i++ ) {
                if(spectrumData[i] > midValue) {
                    midValue = spectrumData[i];
                }
            }
            //midValue /= interval;
            this.midValue = midValue * multiplier;

            float highValue = 0;
            for(int i = interval*2;  i < interval*3; i++ ) {
                if(spectrumData[i] > highValue) {
                    highValue = spectrumData[i];
                }
            }
           // highValue /= interval;
            this.highValue = highValue * multiplier;
        }*/

        float getSimpleRMS() {
            audioSource.GetOutputData(sampleData, 0);
            float total = 0;
            for(int i = 0;  i < samples; i++ ) {
                total += sampleData[i] * sampleData[i];
            }
            return Mathf.Sqrt(total / samples);
        }

        #endregion
    }
}
