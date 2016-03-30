using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TVTracker.Common;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

//Half-inched from https://github.com/jsuarezruiz/Universal-Apps-Samples/blob/master/GesturesBehavior/GesturesBehavior/GesturesBehavior.Shared/ViewModels/Base/ViewModelBase.cs
//Simpler version can be found at http://www.journeyintocode.com/2013/04/mvvm-viewmodelbase.html

namespace TVTracker.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        private Frame _appFrame;
        private bool _isBusy;

        public ViewModelBase()
        {
        }

        public Frame AppFrame
        {
            get { return _appFrame; }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                RaisePropertyChanged();
            }
        }

        //public event CustomEventArgs OnExceptionOccurred;
        public event EventHandler<CustomEventArgs> ExceptionOccurred;
        public void RaiseExceptionOccurred(CustomException exception)
        {
            EventHandler<CustomEventArgs> handler = ExceptionOccurred;
            if (handler != null)
            {
                handler(null, new CustomEventArgs(exception));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        //public abstract Task OnNavigatedFrom(NavigationEventArgs args);

        //public abstract Task OnNavigatedTo(NavigationEventArgs args);

        //public void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        //{
        //    var handler = PropertyChanged;
        //    if (handler != null)
        //        handler(this, new PropertyChangedEventArgs(propertyName));
        //}

        protected virtual void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        internal void SetAppFrame(Frame viewFrame)
        {
            _appFrame = viewFrame;
        }
    }
}
