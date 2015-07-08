using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InversionOfControlContainer.Tests.TestingClasses
{
    public class UserController : IController
    {
        public IRepository Repository { get; private set; }
        public IEmailService EmailService { get; private set; }

        public UserController()
        {

        }

        public UserController(IRepository repository, IEmailService emailService)
        {
            Repository = repository;
            EmailService = emailService;
        }
    }
}
