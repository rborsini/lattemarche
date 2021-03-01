using LatteMarche.Xamarin.Enums;
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
                new SideMenuItem {Id = MenuItemType.Giri, Title="Giri" },
                new SideMenuItem {Id = MenuItemType.Trasbordi, Title="Trasbordi" },
                new SideMenuItem {Id = MenuItemType.Impostazioni, Title="Impostazioni" }
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