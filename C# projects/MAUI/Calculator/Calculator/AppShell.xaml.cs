using ELTE.Calculator.Model;
using ELTE.Calculator.Resources;
using ELTE.Calculator.View;
using ELTE.Calculator.ViewModel;

namespace ELTE.Calculator;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        // model és nézetmodell létrehozása
        CalculatorModel model = new CalculatorModel();
        CalculatorViewModel viewModel = new CalculatorViewModel(model);
        viewModel.ErrorOccured += new EventHandler<ErrorMessageEventArgs>(ViewModel_ErrorOccured);

        BindingContext = viewModel; // nézetmodell befecskendezése
    }

    private async void ViewModel_ErrorOccured(object? sender, ErrorMessageEventArgs e)
    {
        // hibaüzenet megjelenítése
        switch (e.Message)
        {
            case ErrorMessage.FormatError:
                await DisplayAlert(ApplicationText.CalculatorTitle, ApplicationText.FormatErrorMessage + Environment.NewLine + ApplicationText.PleaseCorrectText, ApplicationText.CorrectText);

                break;
            case ErrorMessage.NoNumberError:
                await DisplayAlert(ApplicationText.CalculatorTitle, ApplicationText.NoNumberErrorMessage + Environment.NewLine + ApplicationText.PleaseCorrectText, ApplicationText.CorrectText);
                break;
            case ErrorMessage.OverflowError:
                await DisplayAlert(ApplicationText.CalculatorTitle, ApplicationText.OverflowErrorMessage + Environment.NewLine + ApplicationText.PleaseCorrectText, ApplicationText.CorrectText);
                break;
        }
    }
}
