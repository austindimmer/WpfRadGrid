using System;
using System.Windows;

namespace WPF_Task.DesignTimeData
{
    public class DesignEmployee : Model.Employee
    {
        public DesignEmployee()
        {
            Name = "Austin";
            Department = "Sales";
            PhoneNumber = "+49 (0) 123456789";
        }
    }
}
