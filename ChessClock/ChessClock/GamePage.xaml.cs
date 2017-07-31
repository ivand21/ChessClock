using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
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
    public partial class GamePage : ContentPage
    {
        public enum Side { White, Black };

        public Side ToMove { get; set; }
        public bool IsRunning { get; set; }

        public readonly TimeSpan WhiteStart;
        public readonly TimeSpan BlackStart;

        private TimeSpan _timeWhite;
        private TimeSpan _timeBlack;

        public TimeSpan TimeWhite
        {
            get { return _timeWhite; }
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    _timeWhite = value;
                    lblWhiteTime.Text = ParseTime(value);
                    if (value == TimeSpan.Zero)
                        lblWhiteTime.TextColor = Color.Red;
                });
            }
        }

        public TimeSpan TimeBlack
        {
            get { return _timeBlack; }
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    _timeBlack = value;
                    lblBlackTime.Text = ParseTime(value);
                    if (value == TimeSpan.Zero)
                        lblWhiteTime.TextColor = Color.Red;
                });
            }
        }

        public TimeSpan WhitePerMove { get; set; }
        public TimeSpan BlackPerMove { get; set; }

        public TimeSpan TimeControlBonus { get; set; }

        public int TimeControlMoves { get; set; }
        public int MovesCount { get; set; }

        public GamePage()
        {
            InitializeComponent();
        }

        public GamePage(int wTime, int bTime, int wMove, int bMove, int movesTc, int bonusTc)
        {
            InitializeComponent();

            WhiteStart = TimeSpan.FromMinutes(wTime);
            BlackStart = TimeSpan.FromMinutes(bTime);

            TimeWhite = TimeSpan.FromMinutes(wTime);
            TimeBlack = TimeSpan.FromMinutes(bTime);

            if (bonusTc != 0) TimeControlBonus = TimeSpan.FromMinutes(bonusTc);

            if (wMove != 0) WhitePerMove = TimeSpan.FromSeconds(wMove);
            if (bMove != 0) BlackPerMove = TimeSpan.FromSeconds(bMove);

            TimeControlMoves = movesTc;

            ToMove = Side.White;
            IsRunning = false;
            MovesCount = 0;
        }

        private void OnStartStopButtonClicked(object sender, EventArgs e)
        {
            if (!IsRunning)
            {
                IsRunning = true;
                Device.StartTimer(TimeSpan.FromSeconds(1), OnTimerTick);
            }
            else
            {
                IsRunning = false;
            }
        }

        private bool OnTimerTick()
        {
            if (IsRunning)
            {
                if (ToMove == Side.White)
                {
                    TimeWhite = UpdateTime(TimeWhite);
                }
                else
                {
                    TimeBlack = UpdateTime(TimeBlack);
                }
            }

            if (IsRunning) return true;
            else return false;
        }

        private async void OnGameMenuButtonClicked(object sender, EventArgs e)
        {
            //await Navigation.PopToRootAsync();

            IsRunning = false;
            var popup = new PopupMenu(this);
            await PopupNavigation.PushAsync(popup);
        }

        private async void OnMainMenuButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }


        private void OnWhiteTapped(object sender, EventArgs e)
        {
            if (ToMove == Side.White)
            {
                if (WhitePerMove != null) TimeWhite = TimeWhite.Add(WhitePerMove);
                ToMove = Side.Black;
            }
        }

        private void OnBlackTapped(object sender, EventArgs e)
        {
            if (ToMove == Side.Black)
            {
                MovesCount++;
                if (BlackPerMove != null) TimeBlack = TimeBlack.Add(BlackPerMove);

                if (TimeControlMoves != 0 && MovesCount == TimeControlMoves)
                {
                    TimeWhite = TimeWhite.Add(TimeControlBonus);
                    TimeBlack = TimeBlack.Add(TimeControlBonus);
                }
                ToMove = Side.White;
            }
        }

        private string ParseTime(TimeSpan ts)
        {
            if (ts.Hours == 0)
            {
                return ts.Minutes + ":" + ts.Seconds.ToString("D2");
            }
            else
            {
                return ts.Hours + ":" + ts.Minutes.ToString("D2");
            }
        }

        private TimeSpan UpdateTime(TimeSpan ts)
        {
            var second = TimeSpan.FromSeconds(-1);

            if (IsRunning)
            {
                if (ts != TimeSpan.Zero)
                {
                    ts = ts.Add(second);
                }
            }

            return ts;
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (width != this.Width || height != this.Height)
            {
                this.WidthRequest = width;
                this.HeightRequest = height;
                if (width > height)
                {
                    outerStack.Orientation = StackOrientation.Horizontal;
                    stackWhiteTime.Orientation = StackOrientation.Horizontal;
                    stackBlackTime.Orientation = StackOrientation.Horizontal;
                    stackMenu.Orientation = StackOrientation.Vertical;
                }
                else
                {
                    outerStack.Orientation = StackOrientation.Vertical;
                    stackWhiteTime.Orientation = StackOrientation.Vertical;
                    stackBlackTime.Orientation = StackOrientation.Vertical;
                    stackMenu.Orientation = StackOrientation.Horizontal;
                }
            }
        }

        public void Reset()
        {
            IsRunning = false;
            TimeWhite = new TimeSpan(WhiteStart.Hours, WhiteStart.Minutes, WhiteStart.Seconds);
            TimeBlack = new TimeSpan(BlackStart.Hours, BlackStart.Minutes, BlackStart.Seconds);
            ToMove = Side.White;
            MovesCount = 0;
        }

        public void SwapSides()
        {
            var tmp = new TimeSpan(TimeWhite.Hours, TimeWhite.Minutes, TimeWhite.Seconds);
            TimeWhite = new TimeSpan(TimeBlack.Hours, TimeBlack.Minutes, TimeBlack.Seconds);
            TimeBlack = tmp;
            if (ToMove == Side.White)
                ToMove = Side.Black;
            else
                ToMove = Side.White;
        }

    }
}