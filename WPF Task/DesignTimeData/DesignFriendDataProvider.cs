using System.Collections.Generic;
using WPF_Task.DataProvider;

namespace WPF_Task.DesignTimeData
{
    class DesignEmployeeDataProvider : IEmployeeDataProvider
    {
        public IEnumerable<Model.Employee> LoadEmployees()
        {
            yield return new DesignEmployee();
            yield return new DesignEmployee { Name = "Julia" };
            yield return new DesignEmployee { Name = "Anna" };
            yield return new DesignEmployee { Name = "Sara" };
        }
    }
}
