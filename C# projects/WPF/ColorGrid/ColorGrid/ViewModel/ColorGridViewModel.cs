using System;
using System.Collections.ObjectModel;

namespace ELTE.ColorGrid.ViewModel
{
    /// <summary>
    /// Színrács nézetmodell típusa.
    /// </summary>
    public class ColorGridViewModel : ViewModelBase
    {
        private Int32 _rowCount;
        private Int32 _columnCount;

        /// <summary>
        /// Sorok számának lekérdezée, vagy beállítása.
        /// </summary>
        public Int32 RowCount
        {
            get { return _rowCount; }
            set
            {
                if (_rowCount != value)
                {
                    _rowCount = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Oszlopok számának lekérdezése, vagy beállítása.
        /// </summary>
        public Int32 ColumnCount
        {
            get { return _columnCount; }
            set
            {
                if (_columnCount != value)
                {
                    _columnCount = value;
                    OnPropertyChanged();
                }
            }
        }
        
        /// <summary>
        /// Mezők lekérdezése.
        /// </summary>
        public ObservableCollection<ColorFieldViewModel> Fields { get; private set; }

        /// <summary>
        /// Méretváltás parancsának lekérdezése.
        /// </summary>
        public DelegateCommand ChangeSizeCommand { get; private set; }

        /// <summary>
        /// Színrács nézetmodell példányosítása.
        /// </summary>
        public ColorGridViewModel()
        {
            Fields = new ObservableCollection<ColorFieldViewModel>();

            ChangeSizeCommand = new DelegateCommand(x => GenerateFields());
        }

        /// <summary>
        /// Mezők generálása.
        /// </summary>
        private void GenerateFields()
        {
            Fields.Clear();
            for (Int32 rowIndex = 0; rowIndex < RowCount; rowIndex++)
                for (Int32 columnIndex = 0; columnIndex < ColumnCount; columnIndex++)
                {
                    Fields.Add(new ColorFieldViewModel // mező létrehozása
                    {
                        ColorNumber = 0, // megadjuk a kezdőértékeket
                        Row = rowIndex,
                        Column = columnIndex,
                        FieldChangeCommand = new DelegateCommand(field =>
                        {
                            if (field is ColorFieldViewModel viewModel)
                                FieldChange(viewModel);
                        }) // és a végrehajtandó parancsot
                    });
                }
        }

        /// <summary>
        /// Mezőváltozás kezelése.
        /// </summary>
        /// <param name="selectedField">A kiválasztott mező.</param>
        private void FieldChange(ColorFieldViewModel selectedField)
        {
            Int32 color = (selectedField.ColorNumber + 1) % 3;
            // a rákövetkező színt vesszük

            foreach (ColorFieldViewModel field in Fields)
            {
                if (field.Column == selectedField.Column || field.Row == selectedField.Row) // adott oszlopban és sorban
                    field.ColorNumber = color; // átszínezés végrehajtása
            }
        }
    }
}
