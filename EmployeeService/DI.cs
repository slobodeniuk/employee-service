using EmployeeService.DAL.Configs;
using EmployeeService.DAL.EmployeeRepository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;

namespace EmployeeService
{
    internal class DI
    {
        private static Lazy<DI> _di = new Lazy<DI>(() =>
        {
            var di = new DI();
            di.Init();

            return di;
        });

        public static DI Instance => _di.Value;

        private IServiceProvider _serviceProvider;

        private DI()
        {
        }

        private void Init()
        {
            var services = new ServiceCollection();
            //your di ingections could be here
            services.AddTransient<IEmployeeRepository, EmployeeRepository>();
            services.AddSingleton(new EmployeeRepositorySettings
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["EmployeeDB"].ConnectionString
            });

            this._serviceProvider = services.BuildServiceProvider();
        }

        public T Get<T>()
        {
            return _serviceProvider.GetRequiredService<T>();
        }
    }
}
