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
    public partial class GameSettingsPage : ContentPage
    {
        public GameSettingsPage()
        {
            InitializeComponent();
        }

        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void OnStartButtonClicked(object sender, EventArgs e)
        {
            try
            {
                
                if (Validate())
                {
                    int wTotal = Int32.Parse(WhiteTotal.Text);
                    int bTotal = Int32.Parse(BlackTotal.Text);
                    int wMove = Int32.Parse(WhitePerMove.Text);
                    int bMove = Int32.Parse(BlackPerMove.Text);
                    int movesTc = Int32.Parse(TimeControlMoves.Text);
                    int tcBonus = Int32.Parse(ControlBonusTime.Text);
                    await Navigation.PushAsync(new GamePage(wTotal, bTotal, wMove, bMove, movesTc, tcBonus), true);
                }
                else
                {
                    await DisplayAlert("Error", "Invalid game parameters", "OK");
                }
            }
            catch (Exception)
            {
                await DisplayAlert("Error", "Invalid game parameters", "OK");
            }
        }

        private void OnEqualizeButtonClicked(object sender, EventArgs e)
        {
            BlackTotal.Text = WhiteTotal.Text;
            BlackPerMove.Text = WhitePerMove.Text;
        }

        private bool Validate()
        {
            if (!IsAllDigits(WhiteTotal.Text)) return false;
            if (!IsAllDigits(WhitePerMove.Text)) return false;
            if (!IsAllDigits(BlackTotal.Text)) return false;
            if (!IsAllDigits(BlackPerMove.Text)) return false;
            if (!IsAllDigits(TimeControlMoves.Text)) return false;
            if (!IsAllDigits(ControlBonusTime.Text)) return false;

            return true;
        }

        private bool IsAllDigits(string s)
        {
            var str = s.Cast<char>().ToArray();
            return str.All(char.IsDigit);
        }

    }
}