using System;
using System.Windows;
using System.Windows.Documents;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using ScottIsAFool.WindowsPhone.Tools;
using workout7.Helpers;
using System.Diagnostics;
using Microsoft.Phone.Tasks;

namespace workout7
{
    public partial class MainPage : PhoneApplicationPage
    {
        MarketplaceDetailTask marketplaceDetailTask = new MarketplaceDetailTask();

        private const int TOTAL_EXERCISES = 12;

        private TimeSpan timeSpan;
        private DispatcherTimer dispatcherTimer;
        private DateTime endTime;

        private bool timerLocked;   // simple method, but still does the job ;-) 

        private int exerciseIndex;
        private readonly string[] exerciseNames;
        private readonly string[] imageNames;
        private readonly bool[] switchSides;

        // private int streakCounter;
        // private DateTimeOffset offset = DateTime.Parse("2008-05-01T07:34:42-5:00"); // for sure?

        enum Activity
        {
            GettingReady,
            Exercise,
            Rest
        };

        private Activity currentActivity;

        private Stream stream;
        private SoundEffect soundTick;
        private SoundEffect soundBeep;
        private SoundEffect soundSwitch;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            this.exerciseNames = new string[]{
                "jumping jacks",
                "wall-sit",
                "push up",
                "abdominal crunch",
                "step up on chair",
                "squat",
                "triceps dip",
                "plank",
                "high knees running",
                "lunge",
                "push up and rotation",
                "side plank"
            };

            this.switchSides = new bool[]
            {
                false,
                false,
                false,
                false,
                false,
                false,
                false,
                false,
                false,
                true,
                true,
                true
            };

            this.imageNames = new string[] {
                "exercise.jacks.png", 
                "exercise.wall-sit.png",
                "exercise.push-up.png",
                "exercise.crunch.png",
                "exercise.step-up.png", 
                "exercise.squat.png", 
                "exercise.triceps-dip.png", 
                "exercise.plank.png",
                "exercise.running.png", 
                "exercise.lunge.png", 
                "exercise.push-up-rotate.png", 
                "exercise.side-plank.png"
            };

            stream = TitleContainer.OpenStream("Sounds/Tick.wav");
            soundTick = SoundEffect.FromStream(stream);
            FrameworkDispatcher.Update();

            
            stream = TitleContainer.OpenStream("Sounds/Beep.wav");
            soundBeep = SoundEffect.FromStream(stream);
            FrameworkDispatcher.Update();

            stream = TitleContainer.OpenStream("Sounds/Switch.wav");
            soundSwitch = SoundEffect.FromStream(stream);
            FrameworkDispatcher.Update();

            stream.Close();

            currentActivity = Activity.GettingReady;

            /* Prepare dispatcher Timer */
            if (this.dispatcherTimer == null)
            {
                this.dispatcherTimer = new DispatcherTimer();
                this.dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                this.dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            }

            if (SettingsHelper.IsFirstRun)
            {
                MessageBox.Show("Streak counter will start after finishing the first complete workout");
                SettingsHelper.IsFirstRun = false;
            }
        }

        ~MainPage()
        {
           // stream.Close();
            this.dispatcherTimer = null;
            PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Enabled;
        }

#if DEBUG
        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            TileManager.UpdatePrimaryTile(5);
        }
#endif

        private void NextActivity()
        {
            // lock the lockscreen during workout
            if (PhoneApplicationService.Current.UserIdleDetectionMode != IdleDetectionMode.Disabled)
            {
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

            if (this.dispatcherTimer != null)
            {
                switch (currentActivity)
                {
                    /* In case of practitioner is getting ready to workout just 
                     * set timespan to 5 seconds and label text.
                     */
                    case Activity.GettingReady:
                        this.timeSpan = new TimeSpan(0, 0, 5);
                        this.lExercise.Text = "get ready!";
                        this.image.Source = new BitmapImage(new Uri("Images/rest.png",
                            UriKind.Relative));
                        this.lTimer.Text = "00:05";
                        soundBeep.Play();
                        break;

                    /* In case of activity is a workout set the timespan to 30 secs, label text
                     * and load image from Images/ folder. 
                     */
                    case Activity.Exercise:
                        this.timeSpan = new TimeSpan(0, 0, 30);
                        
                        this.lExercise.Text = this.exerciseNames[this.exerciseIndex];
                        this.image.Source = new BitmapImage(new Uri("Images/" + this.imageNames[this.exerciseIndex],
                            UriKind.Relative));
                        this.lTimer.Text = "00:30";
                        
                        this.soundBeep.Play();
                        break;

                    /* In case of activity is a rest set the timespan to 10 secs 
                     * and the label to combined "rest" word [...]
                     */
                    case Activity.Rest:
                       // this.lExercise.FontSize = 72;
                        if (this.exerciseIndex < TOTAL_EXERCISES - 1)
                        {
                            this.timeSpan = new TimeSpan(0, 0, 10);
                            this.lExercise.Text = "rest";

                            /* [...] and next exercise name with lower font.
                             */
                            this.lExercise.Inlines.Add(new Run
                            {
                                Text = " \u2192" +
                                    this.exerciseNames[this.exerciseIndex + 1],
                                FontSize = 32
                            });
                            this.image.Source = new BitmapImage(new Uri("Images/rest.png",
                            UriKind.Relative));
                            this.lTimer.Text = "00:10";
                            this.soundBeep.Play();
                        }
                        else Finish();
                        break;
                }

                /* set endTime to time after [timeSpan] seconds 
                 */
                if (timerLocked == false)
                {
                    this.endTime = DateTime.Now;
                    this.endTime = this.endTime.Add(timeSpan);
                    this.dispatcherTimer.Start();
                }
            }
        }

        private void Finish()
        {
            if(PhoneApplicationService.Current.UserIdleDetectionMode != IdleDetectionMode.Enabled)
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Enabled;

            this.dispatcherTimer.Stop();
            this.timerLocked = true;

            this.lTimer.Text = "";
            this.lExercise.Text = "";
            this.lExercise.Inlines.Add(new Run { Text = "you've finished!" });
            this.image.Source = new BitmapImage(new Uri("Images/main.png", UriKind.Relative));

            this.soundBeep.Play();

            UpdateStreakCounter(SettingsHelper.CurrentStreak + 1);
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan tmps = (endTime - DateTime.Now);
            this.lTimer.Text = String.Format("00:{1:00}", tmps.Minutes, tmps.Seconds+1);

            if (this.currentActivity == Activity.Exercise)
            {
                if (tmps.Seconds+1 != 30 && tmps.Seconds+1 != 0) this.soundTick.Play();
                
                if (this.switchSides[this.exerciseIndex] == true && tmps.Seconds == 15) this.soundSwitch.Play();
            }

            if (tmps <= TimeSpan.Zero)
            {
                this.dispatcherTimer.Stop();
                
                switch (this.currentActivity)
                {
                    case Activity.GettingReady:
                        this.currentActivity = Activity.Exercise;
                        break;
                    case Activity.Exercise:
                        if (this.exerciseIndex < TOTAL_EXERCISES)
                            this.currentActivity = Activity.Rest;
                        break;
                    case Activity.Rest:
                        this.currentActivity = Activity.Exercise;
                        this.exerciseIndex++;
                        this.soundBeep.Play();
                        break;
                }

                if (this.exerciseIndex < TOTAL_EXERCISES)
                    this.NextActivity();
            }
        }

        private void appbarStartWorkoutClick(object sender, EventArgs e)
        {
            this.timerLocked = false;
            this.exerciseIndex = 0;
            this.currentActivity = Activity.GettingReady;
            this.NextActivity();
        }

        private void aboutMenuBarClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/About.xaml", UriKind.Relative));
        }

        

        private void UpdateStreakCounter(int val)
        {
            SettingsHelper.CurrentStreak = val;
            TileManager.UpdatePrimaryTile(val);
        }

        // the code is very messy right now - this region is only the beginning of cleaning :(
        #region methods

        /// <summary>
        /// on each navigation to the page there is need to check license and modify the page depending on the license
        /// </summary>
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if ((Application.Current as App).IsTrial)
            {
                adDuplexAd.Visibility = Visibility.Visible;
                             
            }
            else
            {
                adDuplexAd.Visibility = Visibility.Collapsed;
                UpdateApplicationBar();   
            }
        }

        /// <summary>
        /// used to show or hide 'Support me' button depending on trial/full version of application
        /// </summary>
        private void UpdateApplicationBar()
        {
            /* It's in try-catch block, because it can raise an exception when we try to remove
             * the button when it is already removed. */
            try
            {

                ApplicationBarIconButton supportMeButton = ApplicationBar.Buttons[1]
                    as ApplicationBarIconButton;    // 'support me' button's index is 1

                if (!(Application.Current as App).IsTrial)
                {
                    ApplicationBar.Buttons.Remove(supportMeButton);
                }
            }
            catch (Exception e)
            {
#if DEBUG
                Debug.WriteLine(e.ToString());
#endif
            }
        }

        #endregion
        #region ui methods (events)

        private void SupportMeButton_Click(object sender, EventArgs e)
        {
            marketplaceDetailTask.Show();

        }

        private void SpreadTheLove_Click(object sender, EventArgs e)
        {
            new PhoneFlipMenu(
                new PhoneFlipMenuAction("via sms...", () =>
                {
                    ShareHelper.ShareViaSMS();
                }),
                new PhoneFlipMenuAction("via email...", () =>
                {
                    ShareHelper.ShareViaEmail();
                }),
                new PhoneFlipMenuAction("via social networks...", () =>
                {
                    ShareHelper.ShareViaSocialMedia();
                })).Show();
        }

        #endregion
    }
}