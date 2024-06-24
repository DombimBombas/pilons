using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pilons
{
    [Transaction(TransactionMode.Manual)]
    public class BreakColumnsCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            var pylons = new FilteredElementCollector(doc)
                .OfClass(typeof(FamilyInstance))
                .ToList();
            var floor = new FilteredElementCollector(doc)
                .OfClass(typeof(Floor))
                .ToList();
            var level = new FilteredElementCollector(doc)
                .OfClass(typeof(Level))
                .ToList();



            return Result.Succeeded;
        }
    }
}
