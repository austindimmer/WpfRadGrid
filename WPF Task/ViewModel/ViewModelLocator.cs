using System;
using System.ComponentModel;
using System.Windows;
using Autofac;
using WPF_Task.DataProvider;
using WPF_Task.DesignTimeData;

namespace WPF_Task.ViewModel
{
    public class ViewModelLocator
    {
        private MainViewModel _mainViewModel;
        public MainViewModel MainViewModel
        {
            get
            {
                bool isDisplayedInDesigner = DesignerProperties.GetIsInDesignMode(new FrameworkElement());
                if (_mainViewModel == null)
                {
                    if (isDisplayedInDesigner)
                    {
                        IEmployeeDataProvider dataProvider = (IEmployeeDataProvider)new DesignEmployeeDataProvider();
                        _mainViewModel = new MainViewModel(dataProvider);
                    }

                }
                return _mainViewModel;
            }
        }
    }
}
