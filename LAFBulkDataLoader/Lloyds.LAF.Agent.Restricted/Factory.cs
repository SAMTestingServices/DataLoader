using System;
using System.Collections.Generic;
using Lloyds.LAF.Agent.Restricted.Contracts.Tasks;
using Lloyds.LAF.Agent.Restricted.Infrastructure;
using Lloyds.LAF.Agent.Restricted.Tasks;
using Lloyds.LAF.Audit.Contracts.Tasks;

namespace Lloyds.LAF.Agent.Restricted
{
    public class Factory
    {
        private static readonly Dictionary<Type, Func<object>> RegisteredTypes = new Dictionary<Type, Func<object>>();

        public static T Resolve<T>()
        {
            if (RegisteredTypes == null || RegisteredTypes.Count == 0)
            {
                BuildRegisteredTypes();
            }

            var type = typeof (T);
            if (RegisteredTypes != null)
            {
                return (T) RegisteredTypes[type]();
            }

            return default(T);
        }

        public static void RegisterSingleton<T>(Func<T> func)
        {
            var singleInstance = func();
            RegisteredTypes[typeof (T)] = () => singleInstance;
        }

        public static void RegisterTransient<T>(Func<T> func)
        {
            RegisteredTypes[typeof (T)] = () => func();
        }

        private static void BuildRegisteredTypes()
        {
            RegisterTransient<IApplicationTask>(() => new ApplicationTask(new ApplicationRepository()));

            RegisterTransient<IUserGroupTask>(() => new UserGroupTask(new UserGroupRepository(),
                                                                      Resolve<IApplicationTask>(),
                                                                      Audit.Factory.Resolve<IAuditTasks>()));

            RegisterTransient<IUserTasks>(() => new UserTasks(new UserRepository()));

            RegisterTransient<ICredentialTasks>(() => new CredentialTasks(new CredentialRepository(), 
                Resolve<IUserGroupTask>(),
                Resolve<IApplicationTask>(),
                Audit.Factory.Resolve<IAuditTasks>()));
        }
    }
}