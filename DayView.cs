using System;
using Terminal.Gui;

namespace tui_cal
{
    public class DayView : View
    {
        private DateTime _date;

        private const int StartHour = 6;

        public DayView(DateTime date)
        {
            _date = date;
        }

        public override void OnDrawContent(Rect contentArea)
        {
            base.OnDrawContent(contentArea);
            DrawDay(contentArea);
        }

        private void DrawDay(Rect contentArea)
        {
            var driver = Application.Driver;
            int y = 0;

            // Date header
            var dateHeader = _date.ToString("dddd, MMMM dd, yyyy");
            int x = (contentArea.Width - dateHeader.Length) / 2;
            Move(x, y);
            driver.AddStr(dateHeader);
            y++;
            y++;

            // Time slots
            for (int hour = StartHour; hour < 24; hour++)
            {
                for (int min = 0; min < 60; min += 30)
                {
                    var time = new TimeSpan(hour, min, 0);
                    var timeString = time.ToString(@"hh\:mm");
                    Move(0, y);
                    driver.AddStr(timeString);
                    driver.AddStr(" ____________________________________________________________");
                    y++;
                }
            }
        }
    }
}
