using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using ReactiveUI;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace WPF_Task.Controls
{
    public class CustomFilter : FilteringControl
    {
        
        public CustomFilter DepartmentFilter { get; set; }
        public FrameworkElement FilterEditor { get; set; }
        public Popup FilterPopup { get; set; }



        public CustomFilter(Telerik.Windows.Controls.GridViewColumn column, Popup popup) : base(column){
            // Apply the line below only when using NoXaml binaries 
            // this.DefaultStyleKey=typeof(MyFilteringControl);
            FilterPopup = popup;
        }





        protected override void OnApplyFilter()
        {
            base.OnApplyFilter();

            var popup = this.ParentOfType<System.Windows.Controls.Primitives.Popup>();
            if (popup != null){
                popup.IsOpen = false;
            }
        }
    }
}
