using Terminal.Gui;
using tui_cal;

Application.Init();
var top = Application.Top;

// Creates the top-level window to show
var win = new Window("TUI Calendar")
{
    X = 0,
    Y = 1, // Leave one row for the toplevel menu

    // By using Dim.Fill(), it will automatically resize without manual intervention
    Width = Dim.Fill(),
    Height = Dim.Fill()
};

var monthView = new MonthView(DateTime.Now)
{
    X = 0,
    Y = 0,
    Width = 35,
    Height = Dim.Fill(),
    ColorScheme = new ColorScheme
    {
        Normal = new Terminal.Gui.Attribute(Terminal.Gui.Color.White, Terminal.Gui.Color.Blue),
        Focus = new Terminal.Gui.Attribute(Terminal.Gui.Color.White, Terminal.Gui.Color.Blue),
        HotNormal = new Terminal.Gui.Attribute(Terminal.Gui.Color.White, Terminal.Gui.Color.Blue),
        HotFocus = new Terminal.Gui.Attribute(Terminal.Gui.Color.White, Terminal.Gui.Color.Blue)
    }
};

var lineView = new View()
{
    X = 35,
    Y = 0,
    Width = 1,
    Height = Dim.Fill()
};

lineView.DrawContent += (Rect content) => {
    for (int i = 0; i < lineView.Bounds.Height; i++)
    {
        lineView.Move(0, i);
        Application.Driver.AddRune('│');
    }
};

var dayView = new DayView(DateTime.Now)
{
    X = 36,
    Y = 0,
    Width = Dim.Fill(),
    Height = Dim.Fill()
};

monthView.SelectedDateChanged += (date) => {
    dayView.Date = date;
};

win.Add(monthView, lineView, dayView);

top.Add(win);
Application.Run();