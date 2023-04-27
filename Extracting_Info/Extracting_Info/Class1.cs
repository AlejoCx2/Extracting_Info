using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Architecture;
using System.Reflection;


namespace Extracting_Info
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Class1 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get application and document objects
            UIApplication uiapp = commandData.Application;
            Document doc = uiapp.ActiveUIDocument.Document;

            //Obtener la vista 3D activa
            View3D view = doc.ActiveView as View3D;
            string msg = "Tengo la vista 3D";
            //TaskDialog.Show("My Dialog", msg);

            // Crear un objeto FilteredElementCollector para obtener los elementos topográficos
            FilteredElementCollector topoCollector = new FilteredElementCollector(doc, view.Id);
            topoCollector.OfCategory(BuiltInCategory.OST_Topography);

            // Obtener una lista de los elementos topográficos
            List<Element> topoElements = topoCollector.ToElements().ToList();

            // TopographySurface river = topoElements[1] as TopographySurface;

            TopographySurface topoSurface = topoCollector.FirstElement() as TopographySurface;
            IList<XYZ> points = topoSurface.GetPoints();
            // Calcular el área de la región topográfica
            double area = 0;

            for (int i = 0; i < points.Count; i++)
            {
                int j = (i + 1) % points.Count;
                double x1 = points[i].X;
                double y1 = points[i].Y;
                double x2 = points[j].X;
                double y2 = points[j].Y;

                area += ((x1 * y2) - (x2 * y1));
            }

            area /= 2;
            area = Math.Abs(area);


            /*int surface = 0;
            List<double> areas = new List<double>();
            List<double> perimetros = new List<double>();
            int curves = 0;

            foreach (Element topoElement in topoElements)
            {
                if (topoElement is TopographySurface)
                {
                    surface++;
                    TopographySurface topoSurface = topoElement as TopographySurface;
                    double a= topoSurface.get_Parameter(BuiltInParameter.HOST_AREA_COMPUTED).AsDouble();
                    areas.Add(a);
                    double p = topoSurface.get_Parameter(BuiltInParameter.HOST_PERIMETER_COMPUTED).AsDouble();
                    perimetros.Add(p);
                }
                else if (topoElement is CurveByPoints)
                {
                    curves++;
                }
            }*/



            /*msg = "Se obtubierion " +topoElements.Count.ToString() + " elementos topograficos" + "\r\n" +
                  surface + " Superficies" + "\r\n" +
                  "\tarea: " + area + "\r\n" +
                  "\tperimetro: " + perimeter+ "\r\n" +
                  curves + " Curvas de puntos";*/
            msg = "Tiene los puntos: " + points.Count + "\r\n" +
                  "Area: " + area + "\r\n" +
                  "Elementos: " + topoElements.Count;
            TaskDialog.Show("My Dialog", msg);

            return Result.Succeeded;
        }
    }
}
