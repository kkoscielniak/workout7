using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace workout7.Helpers
{
    static class ShareHelper
    {
        public static void ShareViaSMS()
        {
            SmsComposeTask smsTask = new SmsComposeTask();
            smsTask.Body = "Do some workout with Workout 7 for Windows Phone! Try it for yourself: http://www.windowsphone.com/en-us/store/app/workout-7/49088235-b91e-4af0-bf63-a3e0f189ccfa";
            smsTask.Show();
        }

        public static void ShareViaEmail()
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask();

            emailComposeTask.Subject = "Try Workout 7 for Windows Phone";
            emailComposeTask.Body = "Do some workout with Workout 7 for Windows Phone! Try it for yourself: http://www.windowsphone.com/en-us/store/app/workout-7/49088235-b91e-4af0-bf63-a3e0f189ccfa";

            emailComposeTask.Show();
        }

        public static void ShareViaSocialMedia()
        {
            ShareStatusTask shareStatusTask = new ShareStatusTask();
            shareStatusTask.Status = "#Workout with Workout7 for #WindowsPhone! Try it http://www.windowsphone.com/en-us/store/app/workout-7/49088235-b91e-4af0-bf63-a3e0f189ccfa";
            shareStatusTask.Show();
        }
    }
}
