using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Whitebox.Containers.Autofac;
using WPF_Task.DataProvider;
using WPF_Task.ViewModel;

namespace WPF_Task
{
    public static class IoCContainer
    {
        public static IContainer BaseContainer { get; private set; }

        public static void Build()
        {
            if (BaseContainer == null)
            {
                var builder = new ContainerBuilder();
                builder.RegisterModule<WhiteboxProfilingModule>();
                builder.RegisterType<EmployeeDataProvider>().As<IEmployeeDataProvider>();
                builder.RegisterType<MainViewModel>();
                builder.RegisterType<MainWindow>();
                BaseContainer = builder.Build();
            }
        }

        public static TService Resolve<TService>()
        {
            return BaseContainer.Resolve<TService>();
        }
    }
}
