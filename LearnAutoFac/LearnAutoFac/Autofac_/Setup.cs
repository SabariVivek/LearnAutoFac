using Autofac;
using NUnit.Framework;

namespace LearnAutoFac.Autofac
{
    public class Setup
    {
        [Test]
        public void SetUpMethod()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<AudioPlayer>().As<IAudioPlayer>();
            builder.RegisterType<MusicLibrary>().As<IMusicLibrary>().SingleInstance();
            var container = builder.Build();

            var music = container.Resolve<IMusicLibrary>();
            music.ListSongs();
        }
    }
}
