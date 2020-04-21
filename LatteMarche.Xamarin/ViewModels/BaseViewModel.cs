using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LatteMarche.Xamarin.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {

        #region Fields

        protected Page page;
        protected INavigation navigation;

        protected bool isBusy = false;
        private string title = string.Empty;

        #endregion

        #region Properties

        public bool IsOnline => Connectivity.NetworkAccess != NetworkAccess.None;

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }


        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Constructor

        public BaseViewModel(INavigation navigation, Page page)
        {
            this.navigation = navigation;
            this.page = page;            
        }

        #endregion

        #region Methods

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
