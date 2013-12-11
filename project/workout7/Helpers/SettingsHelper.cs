using System.IO.IsolatedStorage;

namespace workout7.Helpers
{
    public static class SettingsHelper
    {
        public static bool IsFirstRun
        {
            get
            {
                if (IsolatedStorageSettings.ApplicationSettings.Contains("isFirstRun"))
                {
                    return ((bool)IsolatedStorageSettings.ApplicationSettings["isFirstRun"]);
                }
                else
                    return true;
            }
            set
            {
                if (IsolatedStorageSettings.ApplicationSettings.Contains("isFirstRun"))
                {
                    IsolatedStorageSettings.ApplicationSettings["isFirstRun"] = value;
                }
                else
                {
                    IsolatedStorageSettings.ApplicationSettings.Add("isFirstRun", value);
                }
            }
        }

        public static bool StreakCounterEnabled
        {
            get
            {
                if (IsolatedStorageSettings.ApplicationSettings.Contains("streakCounterEnabled"))
                {
                    return ((bool)IsolatedStorageSettings.ApplicationSettings["streakCounterEnabled"]);
                }
                else
                    return true;
            }
            set
            {
                if (IsolatedStorageSettings.ApplicationSettings.Contains("streakCounterEnabled"))
                {
                    IsolatedStorageSettings.ApplicationSettings["streakCounterEnabled"] = value;
                }
                else
                {
                    IsolatedStorageSettings.ApplicationSettings.Add("streakCounterEnabled", value);
                }
            }
        }

        public static int CurrentStreak
        {
            get
            {
                if (IsolatedStorageSettings.ApplicationSettings.Contains("currentStreak"))
                {
                    return ((int)IsolatedStorageSettings.ApplicationSettings["currentStreak"]);
                }
                else
                    return 0;
            }
            set
            {
                if (IsolatedStorageSettings.ApplicationSettings.Contains("currentStreak"))
                {
                    IsolatedStorageSettings.ApplicationSettings["currentStreak"] = value;
                }
                else
                {
                    IsolatedStorageSettings.ApplicationSettings.Add("currentStreak", value);
                }
            }
        }        
    }
}

