using Autofac;
using NUnit.Framework;

namespace LearnAutofac.Autofac
{
    public class WithAutofac
    {
        public interface ILog
        {
            void Write(string message);
        }

        public interface IReport
        {
            void Report(string message);
        }

        public class ConsoleLog : ILog, IReport
        {
            public void Report(string message)
            {
                Console.WriteLine(message);
            }

            public void Write(string message)
            {
                Console.WriteLine(message);
            }
        }

        public class Emaillog : ILog
        {
            private const string _adminEmail = "sabari@gamil.com";
            public void Write(string message)
            {
                Console.WriteLine($"Email sent to {_adminEmail} : {message}");
            }
        }

        public class Engine
        {
            private ILog log;
            private int id;

            public Engine(ILog log)
            {
                this.log = log;
                id = new Random().Next();
            }

            public void Ahead(int power)
            {
                log.Write($"Engine [{id}] ahead {power}");
            }
        }

        public class Car
        {
            private Engine engine;
            private ILog log;

            public Car(Engine engine)
            {
                this.engine = engine;
                this.log = new Emaillog();
            }

            public Car(Engine engine, ILog log)
            {
                this.engine = engine;
                this.log = log;
            }

            public void Go()
            {
                engine.Ahead(100);
                log.Write("Car going forward...");
            }
        }

        public class Program
        {
            [Test]
            public void SingleTypeRegistration()
            {
                var builder = new ContainerBuilder();
                builder.RegisterType<Emaillog>().As<ILog>().AsSelf();
                builder.RegisterType<Engine>();
                builder.RegisterType<Car>();

                IContainer container = builder.Build();

                var car = container.Resolve<Car>();
                car.Go();
            }

            /**
             * Both Email log and Console log is registrating the same log but the last one would 
             *      get replaced as default...
             * If you don't want to change the default, you can mention "PreserveExistingDefaults()"...
             */
            [Test]
            public void SameTypeRegistration()
            {
                var builder = new ContainerBuilder();
                builder.RegisterType<Emaillog>().As<ILog>();
                builder.RegisterType<ConsoleLog>().As<ILog>().PreserveExistingDefaults();
                builder.RegisterType<Engine>();
                builder.RegisterType<Car>();

                IContainer container = builder.Build();

                var car = container.Resolve<Car>();
                car.Go();
            }

            /**
             * Registration of Same Class With Multiple Interface...
             */
            [Test]
            public void RegistrationSameClassWithMultipleInterface()
            {
                var builder = new ContainerBuilder();
                builder.RegisterType<Emaillog>().As<ILog>().As<IReport>();
                builder.RegisterType<ConsoleLog>().As<ILog>().PreserveExistingDefaults();
                builder.RegisterType<Engine>();
                builder.RegisterType<Car>();

                IContainer container = builder.Build();

                var car = container.Resolve<Car>();
                car.Go();
            }

            /**
             * Handling multiple constructors (Choosing the best one out of it)...
             */
            [Test]
            public void HandlingMultipleConstructor()
            {
                var builder = new ContainerBuilder();
                builder.RegisterType<ConsoleLog>().As<ILog>();
                builder.RegisterType<Engine>();
                builder.RegisterType<Car>().UsingConstructor(typeof(Engine));

                IContainer container = builder.Build();

                var car = container.Resolve<Car>();
                car.Go();
            }

            /**
             * Registing the instance which is already there in our program instead of 
             *      asking the autofac to create one...
             */
            [Test]
            public void RegisteringTheAvailabeInstance()
            {
                // Readymade Instance which we created...
                var log = new ConsoleLog();

                var builder = new ContainerBuilder();
                builder.RegisterInstance(log).As<ILog>();
                builder.RegisterType<Engine>();
                builder.RegisterType<Car>().UsingConstructor(typeof(Engine));

                IContainer container = builder.Build();

                var car = container.Resolve<Car>();
                car.Go();
            }
        }
    }
}