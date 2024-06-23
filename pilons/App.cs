using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Configuration.Assemblies;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace pilons
{
    public class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication application)
        {
            //вкладка
            string tabName = "Вкладка-хуядка";
            application.CreateRibbonTab(tabName);

            //панель на вкладке
            RibbonPanel panel = application.CreateRibbonPanel(tabName, "ТИУ рулит");

            //кнопка на панели
            string assemblyLocation = Assembly.GetExecutingAssembly().Location,
                   iconsDirectoryPath = Path.GetDirectoryName(assemblyLocation) + @"\icons\";
            PushButtonData buttonData = new PushButtonData("btnSplitColumns", "Split Columns", assemblyLocation, "ColumnSplitter.BreakColumnsCommand");
            panel.AddItem(buttonData);

            return Result.Succeeded;

        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }


    }
}
