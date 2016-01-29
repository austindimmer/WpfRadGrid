using System.Collections.Generic;

namespace WPF_Task.DataProvider
{
    public class EmployeeDataProvider : IEmployeeDataProvider
    {
        public IEnumerable<Model.Employee> LoadEmployees()
        {
            return new List<Model.Employee>
            {
                new Model.Employee {Name = "Austin", Department = "Sales", PhoneNumber = "+49 123456789" },
                new Model.Employee {Name = "Julia", Department = "Sales",PhoneNumber = "+49 987654321"},
                new Model.Employee {Name = "Simon", Department = "Accounts", PhoneNumber = "+49 555666777", },
                new Model.Employee {Name = "James", Department = "Accounts", PhoneNumber = "+49 123456789", },
                new Model.Employee {Name = "Sandra", Department = "Accounts", PhoneNumber = "+49 555666777", },
                new Model.Employee {Name = "Mike", Department = "Service", PhoneNumber = "+49 123456789", },
                new Model.Employee {Name = "John", Department = "Service", PhoneNumber = "+49 123456789", },
                new Model.Employee {Name = "Wendy", Department = "Service", PhoneNumber = "+49 123456789", },
                new Model.Employee {Name = "Christine", Department = "Logistics", PhoneNumber = "+49 123456789", },
            };
        }
    }
}
