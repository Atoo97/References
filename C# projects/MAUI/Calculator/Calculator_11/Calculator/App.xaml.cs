using ELTE.Calculator.View;

namespace ELTE.Calculator;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

        MainPage = new AppShell();
    }
}
