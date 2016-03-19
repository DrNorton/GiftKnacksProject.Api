using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Windsor;
using Microsoft.Azure.WebJobs.Host;

namespace GiftKnackMessageAgent
{
    public class JobActivator : IJobActivator
    {
        private readonly IWindsorContainer _container;

        public JobActivator(IWindsorContainer container)
        {
            _container = container;
        }

        public T CreateInstance<T>()
        {
            return _container.Resolve<T>();
        }
    }
}
