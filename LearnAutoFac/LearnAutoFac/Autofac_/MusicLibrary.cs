namespace LearnAutoFac.Autofac
{
    public interface IMusicLibrary
    {
        void ListSongs();
    }

    public class MusicLibrary : IMusicLibrary
    {
        private readonly IAudioPlayer _player;

        public MusicLibrary(IAudioPlayer player)
        {
            _player = player;
        }

        public void ListSongs()
        {
            Console.WriteLine("Listing Songs...");
            _player.Play("En Yesu Unnai Thedugiraar.mp3");
        }
    }
}