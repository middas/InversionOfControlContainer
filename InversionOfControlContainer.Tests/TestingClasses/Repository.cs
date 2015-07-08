using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InversionOfControlContainer.Tests.TestingClasses
{
    public class Repository : IRepository
    {
        private DateTime _CreationTime = DateTime.Now;

        public DateTime CreationTime
        {
            get { return _CreationTime; }
        }
    }
}
