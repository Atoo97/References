using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using ELTE.Calculator.Model;

namespace ELTE.Calculator.ViewModel
{
    /// <summary>
    /// Számológép nézetmodell típusa.
    /// </summary>
    public class CalculatorViewModel : INotifyPropertyChanged
    {
        private CalculatorModel _model;
        private String _numberFieldValue;

        /// <summary>
        /// Beviteli mező szövegének lekérdezése, vagy beállítása.
        /// </summary>
        public String NumberFieldValue
        {
            get { return _numberFieldValue; }
            set
            {
                if (_numberFieldValue != value)
                {
                    _numberFieldValue = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Tizedes elhatároló lekérdezése.
        /// </summary>
        public String DecimalSeparator
        {
            get
            {
                return CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                // a nyelvi környezetnek megfelelő tizedesjelet adjuk vissza (magyar környezetben vessző, angolban pont lesz)
            }
        }

        /// <summary>
        /// Számítások listájának lekérdezése.
        /// </summary>
        public ObservableCollection<String> Calculations { get; private set; }

        /// <summary>
        /// Számítás parancsának lekérdezése, vagy beállítása.
        /// </summary>
        public DelegateCommand CalculateCommand { get; private set; }

        /// <summary>
        /// Tulajdonság változásának eseménye.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Hiba keletkezésének eseménye.
        /// </summary>
        public event EventHandler<ErrorMessageEventArgs>? ErrorOccured;

        /// <summary>
        /// Számológép nézetmodell példányosítása.
        /// </summary>
        /// <param name="model">A számológép modell.</param>
        public CalculatorViewModel(CalculatorModel model)
        {
            CalculateCommand = new DelegateCommand(param => Calculate(param?.ToString() ?? ""));
            Calculations = new ObservableCollection<String>();

            _model = model;
            _model.CalculationPerformed += new EventHandler<CalculatorEventArgs>(Model_CalculationPerformed);
            _numberFieldValue = "0";
        }

        /// <summary>
        /// Számítás végrehajtásának eseménykezelője.
        /// </summary>
        /// <param name="sender">Küldő</param>
        /// <param name="e">Eseményargumentumok.</param>
        private void Model_CalculationPerformed(object? sender, CalculatorEventArgs e)
        {
            NumberFieldValue = e.Result.ToString(); // eredmény és művelet kiírása

            if (!String.IsNullOrEmpty(e.CalculationString))
                Calculations.Insert(0, e.CalculationString);
        }

        /// <summary>
        /// Számítás végrehajtása.
        /// </summary>
        /// <param name="operatorString">A művelet szöveges megfelelője.</param>
        private void Calculate(String operatorString)
        {
            try
            {
                Double value = Double.Parse(_numberFieldValue); // szám lekérése

                switch (operatorString)
                // művelet végrehajtása, amely most már számjegy, tizedesjel és törlés is lehet
                {
                    case "+":
                        _model.Calculate(value, Operation.Add);
                        NumberFieldValue = "0";
                        break;
                    case "-":
                        _model.Calculate(value, Operation.Subtract);
                        NumberFieldValue = "0";
                        break;
                    case "*":
                        _model.Calculate(value, Operation.Multiply);
                        NumberFieldValue = "0";
                        break;
                    case "/":
                        _model.Calculate(value, Operation.Divide);
                        NumberFieldValue = "0";
                        break;
                    case "=":
                        _model.Calculate(value, Operation.None);
                        break;
                    default:
                        if (NumberFieldValue == "0" && operatorString != DecimalSeparator)
                            // 0 esetén lecseréljük a tartalmat (kivéve, ha tizedesjel jön), egyébként hozzáírjuk
                            NumberFieldValue = operatorString;
                        else
                            // minden más esetben csak hozzáírjuk 
                            NumberFieldValue += operatorString;
                        break;
                }
            }
            catch (OverflowException)
            {
                // a hibákat most már eseményekkel jelezzük, és a vezérlés majd 
                OnErrorOccured(ErrorMessage.OverflowError);
            }
            catch (FormatException)
            {
                OnErrorOccured(ErrorMessage.FormatError);
            }
            catch (NullReferenceException)
            {
                OnErrorOccured(ErrorMessage.NoNumberError);
            }
        }

        /// <summary>
        /// Tulajdonságváltozás eseménykliváltása.
        /// </summary>
        /// <param name="property">A tulajdonság neve.</param>
        private void OnPropertyChanged([CallerMemberName] String? property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        /// <summary>
        /// Hiba keletkezésének eseménykiváltása.
        /// </summary>
        /// <param name="message">Az üzenet.</param>
        private void OnErrorOccured(ErrorMessage message)
        {
            ErrorOccured?.Invoke(this, new ErrorMessageEventArgs(message));
        }
    }
}
