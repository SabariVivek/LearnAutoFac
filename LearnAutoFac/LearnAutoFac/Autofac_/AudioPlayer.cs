namespace LearnAutoFac.Autofac
{
    public interface IAudioPlayer
    {
        void Play(String song_name);
    }

    public class AudioPlayer : IAudioPlayer
    {
        public void Play(String song_name)
        {
            Console.WriteLine("Playing : \"" + song_name + "\" song...");
        }
    }
}