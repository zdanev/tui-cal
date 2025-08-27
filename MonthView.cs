
using System;
using System.Globalization;
using Terminal.Gui;

namespace tui_cal
{
    public class MonthView : View
    {
        private DateTime _date;
        private DateTime _selectedDate;

        public event Action<DateTime>? SelectedDateChanged;

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                SelectedDateChanged?.Invoke(_selectedDate);
                SetNeedsDisplay();
            }
        }

        public MonthView(DateTime date)
        {
            _date = date;
            _selectedDate = date;
            CanFocus = true;
        }

        public override bool ProcessKey(KeyEvent keyEvent)
        {
            switch (keyEvent.Key)
            {
                case Key.CursorUp:
                    SelectedDate = SelectedDate.AddDays(-7);
                    return true;
                case Key.CursorDown:
                    SelectedDate = SelectedDate.AddDays(7);
                    return true;
                case Key.CursorLeft:
                    SelectedDate = SelectedDate.AddDays(-1);
                    return true;
                case Key.CursorRight:
                    SelectedDate = SelectedDate.AddDays(1);
                    return true;
            }
            return base.ProcessKey(keyEvent);
        }

        public override void OnDrawContent(Rect contentArea)
        {
            base.OnDrawContent(contentArea);
            DrawMonth(contentArea);
        }

        private void DrawMonth(Rect contentArea)
        {
            var driver = Application.Driver;
            int x = 0;
            int y = 0;

            // Month and year header
            var monthYear = _date.ToString("MMMM yyyy");
            x = (contentArea.Width - monthYear.Length) / 2;
            Move(x, y);
            driver.AddStr(monthYear);
            y++;

            // Day of week headers
            x = 0;
            y++;
            var dayNames = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedDayNames;
            for (int i = 0; i < 7; i++)
            {
                var dayName = dayNames[(i + (int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek) % 7];
                Move(x, y);
                driver.AddStr($" {dayName} ");
                x += 5;
            }
            y++;

            var firstDayOfMonth = new DateTime(_date.Year, _date.Month, 1);
            var startingDay = (int)firstDayOfMonth.DayOfWeek - (int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            if (startingDay < 0) startingDay += 7;

            var daysInMonth = DateTime.DaysInMonth(_date.Year, _date.Month);

            x = startingDay * 5;
            for (int day = 1; day <= daysInMonth; day++)
            {
                Move(x, y);
                var date = new DateTime(_date.Year, _date.Month, day);

                if (date.Date == SelectedDate.Date && date.Date == DateTime.Now.Date)
                {
                    var currentAttribute = driver.GetAttribute();
                    var combinedAttribute = new Terminal.Gui.Attribute(Terminal.Gui.Color.BrightRed, currentAttribute.Foreground);
                    driver.SetAttribute(combinedAttribute);
                    driver.AddStr($"{day,3} ");
                    driver.SetAttribute(currentAttribute);
                }
                else if (date.Date == SelectedDate.Date)
                {
                    var currentAttribute = driver.GetAttribute();
                    var reverseAttribute = new Terminal.Gui.Attribute(currentAttribute.Background, currentAttribute.Foreground);
                    driver.SetAttribute(reverseAttribute);
                    driver.AddStr($"{day,3} ");
                    driver.SetAttribute(currentAttribute);
                }
                else if (date.Date == DateTime.Now.Date)
                {
                    var currentAttribute = driver.GetAttribute();
                    var highlightAttribute = new Terminal.Gui.Attribute(Terminal.Gui.Color.BrightRed, currentAttribute.Background);
                    driver.SetAttribute(highlightAttribute);
                    driver.AddStr($"{day,3} ");
                    driver.SetAttribute(currentAttribute);
                }
                else
                {
                    driver.AddStr($"{day,3} ");
                }
                x += 5;
                if ((x / 5 + (int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek) % 7 == (int)CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek)
                {
                    y++;
                    x = 0;
                }
            }
        }
    }
}
