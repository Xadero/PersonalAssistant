using NAudio.Wave;

namespace SpeechRecognizer.Service
{
    public class SoundService
    {
        public static void PlaySound(string filePath)
        {
            WaveOut waveOut = new WaveOut();
            WaveFileReader mp3FileReader;
            mp3FileReader = new WaveFileReader(filePath);
            var x = WaveFormatConversionStream.CreatePcmStream(mp3FileReader);

            waveOut.Init(x);
            waveOut.Play();
        }
    }
}
