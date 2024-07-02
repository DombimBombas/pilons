using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.Windows;
using System.Linq;


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
            foreach (var column in _columnsToBreak)
            {
                SplitColumn(column);
            }
            this.Close();
        }

        private void SplitColumn(ColumnToBreak column)
        {
            Document doc = column.Column.Document; 
            using (Transaction trans = new Transaction(doc, "Разделение пилона"))
            {
                trans.Start();

                ElementId originalColumnId = column.Column.Id;
                LocationPoint originalLocation = column.Column.Location as LocationPoint;

                Parameter baseLevelParam = column.Column.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_PARAM);
                Parameter topLevelParam = column.Column.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_PARAM);

                if (baseLevelParam == null || topLevelParam == null)
                {
                    return;
                }

                ElementId baseLevelId = baseLevelParam.AsElementId();
                ElementId topLevelId = topLevelParam.AsElementId();

                Level baseLevel = doc.GetElement(baseLevelId) as Level;
                Level topLevel = doc.GetElement(topLevelId) as Level;

                var levelCollector = new FilteredElementCollector(doc)
                    .OfClass(typeof(Level))
                    .OrderBy(level => (level as Level).Elevation)
                    .ToList();

                int baseLevelIndex = levelCollector.IndexOf(baseLevel);
                int topLevelIndex = levelCollector.IndexOf(topLevel);

                Element previousColumn = column.Column;

                for (int i = baseLevelIndex; i < topLevelIndex; i++)
                {
                    Level currentBaseLevel = levelCollector[i] as Level;
                    Level currentTopLevel = levelCollector[i + 1] as Level;

                    ElementId newColumnId = ElementTransformUtils.CopyElement(doc, originalColumnId, XYZ.Zero).FirstOrDefault();
                    Element newColumn = doc.GetElement(newColumnId);

                    newColumn.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_PARAM).Set(currentBaseLevel.Id);
                    newColumn.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_PARAM).Set(currentTopLevel.Id);

                    newColumn.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_PARAM).Set(-200);

                    previousColumn = newColumn;
                }

                doc.Delete(originalColumnId);

                trans.Commit();
            }
        }
    }
}
