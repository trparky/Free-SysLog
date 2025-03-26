using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Free_SysLog.NotificationLimiter
{
    public static class NotificationLimiterModule
    {
        public static Dictionary<string, DateTime> lastNotificationTime = new Dictionary<string, DateTime>((int)StringComparison.OrdinalIgnoreCase);
    }

    public class NotificationLimiter
    {
        // Time after which an unused entry is considered stale (in minutes)
        private const int CleanupThresholdInMinutes = 10;

        public void ShowNotification(string tipText, ToolTipIcon tipIcon, string strLogText, string strLogDate, string strSourceIP, string strRawLogText)
        {
            // Get the current time
            var currentTime = DateTime.Now;

            lock (NotificationLimiterModule.lastNotificationTime)
            {
                // Clean up old notification entries
                CleanUpOldEntries(currentTime);

                // Check if this message has been shown recently
                if (NotificationLimiterModule.lastNotificationTime.ContainsKey(tipText))
                {
                    var lastTime = NotificationLimiterModule.lastNotificationTime[tipText];
                    var timeSinceLastNotification = currentTime - lastTime;

                    // If the message was shown within the time limit, do not show it again
                    if (timeSinceLastNotification.TotalSeconds < My.MySettingsProperty.Settings.TimeBetweenSameNotifications)
                        return;
                }

                // Update the last shown time for this message
                NotificationLimiterModule.lastNotificationTime[tipText] = currentTime;
            }

            SupportCode.SupportCode.ShowToastNotification(tipText, tipIcon, strLogText, strLogDate, strSourceIP, strRawLogText);
        }

        // Function to clean up old notification entries
        private void CleanUpOldEntries(DateTime currentTime)
        {
            var keysToRemove = NotificationLimiterModule.lastNotificationTime.Where((kvp) => (currentTime - kvp.Value).TotalMinutes > CleanupThresholdInMinutes).Select((kvp) => kvp.Key).ToList();

            foreach (string key in keysToRemove)
                NotificationLimiterModule.lastNotificationTime.Remove(key);
        }
    }
}