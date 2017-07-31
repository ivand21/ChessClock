using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChessClock
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupMenu : PopupPage
    {
        public PopupMenu(GamePage parent)
        {
            _game = parent;
            InitializeComponent();
        }

        GamePage _game;

        private async void OnMainMenuButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync();
            await Navigation.PopToRootAsync();
        }

        private async void OnResumeButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }

        private async void OnSwapSidesButtonClicked(object sender, EventArgs e)
        {
            _game.SwapSides();
            await Navigation.PopPopupAsync();
        }

        private async void OnResetGameButtonClicked(object sender, EventArgs e)
        {
            _game.Reset();
            await Navigation.PopPopupAsync();
        }

        protected override bool OnBackgroundClicked()
        {
            return base.OnBackgroundClicked();
        }


    }
}