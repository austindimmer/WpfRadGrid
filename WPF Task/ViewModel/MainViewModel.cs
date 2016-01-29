#region

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using System.Threading;
using System.Windows.Controls;
using ReactiveUI;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using WPF_Task.Controls;
using WPF_Task.DataProvider;
using WPF_Task.Model;

#endregion

namespace WPF_Task.ViewModel{

    public class MainViewModel : ReactiveObject{

        private readonly IEmployeeDataProvider _dataProvider;
        private RadGridView _radGridView;
        public BehaviorSubject<bool> ActivatedWithFilterButton;
        public BehaviorSubject<bool> CanOpenStatus;
        public BehaviorSubject<bool> CheckPopupStatusToggle;
        public BehaviorSubject<int> FilterButtonTimesClicked;
        public BehaviorSubject<bool> IsPopupOpen;

        public MainViewModel(IEmployeeDataProvider dataProvider){
            _dataProvider = dataProvider;
            Employees = new ObservableCollection<Employee>();
            LoadData();
            ActivatedWithFilterButton = new BehaviorSubject<bool>(true);
            CanOpenStatus = new BehaviorSubject<bool>(false);
            IsPopupOpen = new BehaviorSubject<bool>(false);
            CheckPopupStatusToggle = new BehaviorSubject<bool>(true);
            FilterButtonTimesClicked = new BehaviorSubject<int>(0);
            CanManuallyOpenPopup = new ObservableAsPropertyHelper<bool>(CheckPopupStatusToggle, _ => { ManuallyOpenPopupStatusChanged(); }, false, RxApp.MainThreadScheduler);
            CanManuallyClosePopup = new ObservableAsPropertyHelper<bool>(CheckPopupStatusToggle, _ => { ManuallyOpenPopupStatusChanged(); }, false, RxApp.MainThreadScheduler);
            PopupOpenedTimesCounter = new ObservableAsPropertyHelper<int>(FilterButtonTimesClicked, _ => { CheckToogleFilterPopupVisibility(); }, 0, RxApp.MainThreadScheduler);

            // Apply the line below only when using NoXaml binaries 
            // this.DefaultStyleKey=typeof(MyFilteringControl);
        }


        public ObservableAsPropertyHelper<bool> CanManuallyOpenPopup { get; set; }
        public ObservableAsPropertyHelper<bool> CanManuallyClosePopup { get; set; }
        public ObservableAsPropertyHelper<int> PopupOpenedTimesCounter { get; set; }
        public CustomFilter CustomFilterInstance { get; set; }
        public RadGridView RadGridView
        {
            get { return _radGridView; }
            set
            {
                _radGridView = value;
                _radGridView.BeginningEdit += _radGridView_BeginningEdit;
                _radGridView.CellEditEnded += _radGridView_CellEditEnded;
                _radGridView.Deleted += _radGridView_Deleted;
                _radGridView.Filtered += _radGridView_Filtered;
                SetupObservableEvents();
            }
        }

        public ObservableCollection<Employee> Employees { get; set; }

        public Employee SelectedEmployee { get; set; }

        public void TracePopupCreationValues(string callingMethod){
            Debug.WriteLine("{0} CanManuallyOpenPopup = {1}", callingMethod, CanManuallyOpenPopup.Value);
            Debug.WriteLine("{0} CanManuallyClosePopup = {1}", callingMethod, CanManuallyClosePopup.Value);
            Debug.WriteLine("{0} IsPopupOpen = {1}", callingMethod, IsPopupOpen.Value);
            Debug.WriteLine("{0} ActivatedWithFilterButton = {1}", callingMethod, ActivatedWithFilterButton.Value);
            Debug.WriteLine("{0} PopupOpenedTimesCounter = {1}", callingMethod, PopupOpenedTimesCounter.Value);
        }

        private void _radGridView_BeginningEdit(object sender, GridViewBeginningEditRoutedEventArgs e){
            if (e.Cell.Column.UniqueName == "Name"){
                ToolTipService.SetToolTip(e.Cell, "Editing the Name may cause changes in the database");
            }
        }

        private void _radGridView_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e){
            // Perform some save persistence operations in the database
            Debug.WriteLine("Saved record to Database");
        }

        private void _radGridView_Deleted(object sender, GridViewDeletedEventArgs e){
            // Perform some delete persistence operations in the database
            Debug.WriteLine("Deleted record from Database");
        }

        private void _radGridView_Filtered(object sender, GridViewFilteredEventArgs e){
            //perform some custom filtering operations here
            Debug.WriteLine("Applied Filter");
        }

        private void CheckToogleFilterPopupVisibility(){
            TracePopupCreationValues(MethodBase.GetCurrentMethod()
                .Name);
            Debug.WriteLine("ManagedThreadId: " + Thread.CurrentThread.ManagedThreadId);
            //ActivatedWithFilterButton.OnNext(true);
            if (CustomFilterInstance != null && CustomFilterInstance.FilterPopup != null){
                var justClosed = false;
                if (CanOpenStatus.Value == false){
                    CustomFilterInstance.FilterPopup.IsOpen = false;
                    justClosed = true;
                    IsPopupOpen.OnNext(false);
                    CanOpenStatus.OnNext(true);
                }
                if (CanOpenStatus.Value && justClosed == false){
                    CustomFilterInstance.FilterPopup.IsOpen = true;
                    IsPopupOpen.OnNext(true);
                    CanOpenStatus.OnNext(false);
                }
                ActivatedWithFilterButton.OnNext(false);
                //TracePopupCreationValues(MethodBase.GetCurrentMethod().Name);
            }
        }

        private void LoadData(){
            var employees = _dataProvider.LoadEmployees();
            foreach (var employee in employees){
                Employees.Add(employee);
            }

            SelectedEmployee = Employees.Count > 0 ? Employees.First() : null;
        }

        private void ManuallyOpenPopupStatusChanged(){
            bool justToggled = false;
            if (IsPopupOpen.Value == false && ActivatedWithFilterButton.Value){
                CanOpenStatus.OnNext(true);
            }
            if (IsPopupOpen.Value && ActivatedWithFilterButton.Value){
                CanOpenStatus.OnNext(false);
                justToggled = true;
            }
            if (IsPopupOpen.Value && justToggled == false )
            {
                CanOpenStatus.OnNext(false);
            }
        }

        private void SetupObservableEvents(){
            //public event EventHandler<EditorCreatedEventArgs> FieldFilterEditorCreated;
            // To consume SimpleEvent as an IObservable:
            var eventAsObservable = Observable.FromEventPattern<EditorCreatedEventArgs>(ev => RadGridView.FieldFilterEditorCreated += ev, ev => RadGridView.FieldFilterEditorCreated -= ev);

            Console.WriteLine("Subscribe");
            //Create two event subscribers
            var FieldFilterEditorCreatedSubscriberOne = eventAsObservable.Subscribe(args =>  FieldFilterEditorCreatedHandler(args));


        }

        private void FieldFilterEditorCreatedHandler(EventPattern<EditorCreatedEventArgs> args){
            var column = args.EventArgs.Column;
            var editor = args.EventArgs.Editor;
            IsPopupOpen.OnNext(true);
            TracePopupCreationValues(MethodBase.GetCurrentMethod()
                .Name);
            CustomFilterInstance.FilterEditor = editor;
        }
    }
}