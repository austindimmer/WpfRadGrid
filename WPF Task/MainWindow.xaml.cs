using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls.Primitives;
using MahApps.Metro.Controls;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using WPF_Task.Controls;
using WPF_Task.ViewModel;

namespace WPF_Task{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow{
        private readonly MainViewModel _mainViewModel;

        public MainWindow(MainViewModel mainViewModel){
            _mainViewModel = mainViewModel;
            // Asuring the commands are properly initialized in the grid itself
            RuntimeHelpers.RunClassConstructor(typeof (RadGridViewCommands).TypeHandle);
            InitializeComponent();
            DataContext = mainViewModel;
        }

        private void Create_Click(object sender, RoutedEventArgs e){
            var departmentColumn = RadGridView1.Columns["Department"];
            var departmentFilter = departmentColumn.ColumnFilterDescriptor;
            departmentFilter.Clear();
        }


        private void FilterButton_Click(object sender, RoutedEventArgs e){
            _mainViewModel.ActivatedWithFilterButton.OnNext(true);
            var current = _mainViewModel.FilterButtonTimesClicked.Value;
            var next = ++current;
            _mainViewModel.FilterButtonTimesClicked.OnNext(next);
            _mainViewModel.TracePopupCreationValues(MethodBase.GetCurrentMethod()
                .Name);
            var justTriggeredChange = false;
            if (_mainViewModel.CheckPopupStatusToggle.Value){
                _mainViewModel.CheckPopupStatusToggle.OnNext(false);
                justTriggeredChange = true;
            }
            if (_mainViewModel.CheckPopupStatusToggle.Value == false && justTriggeredChange == false){
                _mainViewModel.CheckPopupStatusToggle.OnNext(true);
            }
        }

        private void FindPopups(){
            foreach (var headerCell in RadGridView1.ChildrenOfType<GridViewHeaderCell>()){
                var popUp = headerCell.ChildrenOfType<Popup>()
                    .FirstOrDefault();
                // we only have once possible pop up here in this simple case so no complex handling code
                if (popUp != null){
                    var filterInstance = new CustomFilter(RadGridView1.Columns["Department"], popUp);
                    _mainViewModel.CustomFilterInstance = filterInstance;
                    RadGridView1.Columns["Department"].FilteringControl = _mainViewModel.CustomFilterInstance;
                }
            }
        }


        private void RadGridView1_Loaded(object sender, RoutedEventArgs e){
            _mainViewModel.RadGridView = sender as RadGridView;
            Dispatcher.BeginInvoke(new Action(() => FindPopups()));
        }
    }
}