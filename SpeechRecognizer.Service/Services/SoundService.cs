using NAudio.Wave;
using PersonalAssistant.Service.Interfaces;

namespace PersonalAssistant.Service.Services
{
    public class SoundService : ISoundService
    {
        public void PlaySound(string filePath)
        {
            var waveOut = new WaveOut();
            var mp3FileReader = new WaveFileReader(filePath);
            var x = WaveFormatConversionStream.CreatePcmStream(mp3FileReader);

            waveOut.Init(x);
            waveOut.Play();
        }
    }
}
