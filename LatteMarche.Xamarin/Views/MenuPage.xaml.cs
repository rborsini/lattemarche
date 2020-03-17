using LatteMarche.Xamarin.Enums;
using LatteMarche.Xamarin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LatteMarche.Xamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {

        private MainPage RootPage { get => Application.Current.MainPage as MainPage; }

        public List<SideMenuItem> Items;

        public MenuPage()
        {
            InitializeComponent();

            this.Items = new List<SideMenuItem>
            {
                new SideMenuItem {Id = MenuItemType.Prelievi, Title="Prelievi" },
                new SideMenuItem {Id = MenuItemType.Synch, Title="Sincronizzazione" },
                new SideMenuItem {Id = MenuItemType.Login, Title="Login" }
            };
            
            this.ListViewMenu.ItemsSource = this.Items;
            this.ListViewMenu.SelectedItem = null;
            this.ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                var id = (int)((SideMenuItem)e.SelectedItem).Id;
                await RootPage.NavigateFromMenu(id);
            };
        }


    }
}