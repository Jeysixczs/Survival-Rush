using NAudio.Vorbis;
using NAudio.Wave;
using System;
using System.IO;

public static class AudioManager
{
    private static WaveOutEvent outputDevice;
    private static VorbisWaveReader audioFile;
    private static MemoryStream currentMusicStream;

    public static void PlayMusic(byte[] musicData, float volume = 0.5f, bool loop = true)
    {
        StopMusic();

        try
        {
            currentMusicStream = new MemoryStream(musicData);
            audioFile = new VorbisWaveReader(currentMusicStream);
            outputDevice = new WaveOutEvent();

            if (loop)
            {
                outputDevice.PlaybackStopped += (s, e) =>
                {
                    if (currentMusicStream != null) // Check if music wasn't stopped manually
                    {
                        currentMusicStream.Position = 0;
                        audioFile.Position = 0;
                        outputDevice.Play();
                    }
                };
            }

            outputDevice.Init(audioFile);
            outputDevice.Volume = volume;
            outputDevice.Play();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error playing music: {ex.Message}");
            StopMusic();
        }
    }

    public static void StopMusic()
    {
        outputDevice?.Stop();
        audioFile?.Dispose();
        outputDevice?.Dispose();
        currentMusicStream?.Dispose();

        outputDevice = null;
        audioFile = null;
        currentMusicStream = null;
    }


}