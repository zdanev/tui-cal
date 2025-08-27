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
    Height = 10
};

win.Add(monthView);

top.Add(win);
Application.Run();