using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using pilons;
using System.Collections.Generic;
using System.Linq;

[Transaction(TransactionMode.Manual)]
public class BreakColumnsCommand : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        UIDocument uidoc = commandData.Application.ActiveUIDocument;
        Document doc = uidoc.Document;

        var pylons = new FilteredElementCollector(doc)
            .OfClass(typeof(FamilyInstance))
            .ToList();

        List<ColumnToBreak> columnsToBreak = new List<ColumnToBreak>();

        foreach (Element element in pylons)
        {
            int partsCount = DeterminePartsCount(element);
            columnsToBreak.Add(new ColumnToBreak(element, partsCount));
        }

        BreakColumnsView view = new BreakColumnsView(columnsToBreak);
        view.ShowDialog();

        return Result.Succeeded;
    }
    private int DeterminePartsCount(Element column)
    {
        Parameter topLevel = column.get_Parameter(BuiltInParameter.FAMILY_TOP_LEVEL_PARAM);
        Parameter baseLevel = column.get_Parameter(BuiltInParameter.FAMILY_BASE_LEVEL_PARAM);

        return topLevel.AsElementId().IntegerValue - baseLevel.AsElementId().IntegerValue;   //вот тут ошибку выдаёт: System.NullReferenceException: "Ссылка на объект не указывает на экземпляр объекта."  topLevel было null.

    }
}