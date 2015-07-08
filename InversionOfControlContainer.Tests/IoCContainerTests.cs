using InversionOfControlContainer.Tests.TestingClasses;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InversionOfControlContainer.Tests
{
    [TestFixture]
    public class IoCContainerTests
    {
        [Test]
        public void RegisterType()
        {
            try
            {
                using (IoCContainer container = new IoCContainer())
                {
                    container.Register<IRepository, Repository>();
                }
            }
            catch
            {
                Assert.Fail();
            }
        }

        [Test]
        public void Resolve()
        {
            using (IoCContainer container = new IoCContainer())
            {
                container.Register<IRepository, Repository>();

                var result = container.Resolve<IRepository>();

                Assert.IsTrue(result is IRepository);
                Assert.IsTrue(result is Repository);
            }
        }

        [Test]
        public void NonRegisteredResolve()
        {
            using (IoCContainer container = new IoCContainer())
            {
                Assert.Throws(typeof(TypeNotRegisteredException), () => container.Resolve<IRepository>());
            }
        }

        [Test]
        public void RegisterSingletonType()
        {
            using (IoCContainer container = new IoCContainer())
            {
                container.Register<IRepository, Repository>(LifeStyleType.Singleton);

                DateTime created = container.Resolve<IRepository>().CreationTime;

                // sleep for a time to ensure dates change on creation
                Thread.Sleep(1);

                Assert.IsTrue(created == container.Resolve<IRepository>().CreationTime);
            }
        }

        [Test]
        public void RegisterTransientTypeExplicit()
        {
            using (IoCContainer container = new IoCContainer())
            {
                container.Register<IRepository, Repository>(LifeStyleType.Transient);

                DateTime created = container.Resolve<IRepository>().CreationTime;

                // sleep for a time to ensure dates change on creation
                Thread.Sleep(1);

                Assert.IsFalse(created == container.Resolve<IRepository>().CreationTime);
            }
        }

        [Test]
        public void RegisterTransientTypeImplicit()
        {
            using (IoCContainer container = new IoCContainer())
            {
                container.Register<IRepository, Repository>();

                DateTime created = container.Resolve<IRepository>().CreationTime;

                // sleep for a time to ensure dates change on creation
                Thread.Sleep(1);

                Assert.IsFalse(created == container.Resolve<IRepository>().CreationTime);
            }
        }

        [Test]
        public void RegisterTypeWithParameters()
        {
            using (IoCContainer container = new IoCContainer())
            {
                container.Register<IRepository, Repository>();
                container.Register<IEmailService, EmailService>();
                container.Register<IController, UserController>();

                IController controller = container.Resolve<IController>();

                Assert.IsTrue(controller is UserController);

                UserController user = (UserController)controller;

                Assert.IsNotNull(user.EmailService);
                Assert.IsNotNull(user.Repository);
            }
        }

        [Test]
        public void RegisterTypeWithParametersNoRegistered()
        {
            using (IoCContainer container = new IoCContainer())
            {
                container.Register<IController, UserController>();

                IController controller = container.Resolve<IController>();

                Assert.IsTrue(controller is UserController);

                UserController user = (UserController)controller;

                Assert.IsNull(user.EmailService);
                Assert.IsNull(user.Repository);
            }
        }
    }
}
