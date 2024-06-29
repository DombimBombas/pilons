using Autodesk.Revit.DB;

namespace pilons
{
    public class ColumnToBreak
    {
        public Element Column {get;}
        public int PartsCount {get;}

        public ColumnToBreak(Element column, int partsCount)
        {
            Column = column;
            PartsCount = partsCount;
        }
    }
}
