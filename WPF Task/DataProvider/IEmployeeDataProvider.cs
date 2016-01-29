using System.Collections.Generic;

namespace WPF_Task.DataProvider
{
    public interface IEmployeeDataProvider
    {
        IEnumerable<Model.Employee> LoadEmployees();
    }
}