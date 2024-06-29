using System.Collections.Generic;
using System.Windows;


namespace pilons
{
    /// <summary>
    /// Логика взаимодействия для BreakColumnsView.xaml
    /// </summary>
    public partial class BreakColumnsView : Window
    {
        private List<ColumnToBreak> _columnsToBreak;

        public BreakColumnsView(List<ColumnToBreak> columnsToBreak)
        {
            InitializeComponent();
            _columnsToBreak = columnsToBreak;
            ColumnsDataGrid.ItemsSource = _columnsToBreak;
        }


        private void OnSplitColumnsButtonClick(object sender, RoutedEventArgs e)
        {
            // Логика разделения пилонов по частям
            foreach (var column in _columnsToBreak)
            {
                SplitColumn(column);
            }
            this.Close();
        }

        private void SplitColumn(ColumnToBreak column)
        {
            
        }
    }
}
