using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace GetAreaValue
{
    public class WallParams { 
        public int id { get; set; }
        public double volume { get; set; }
        public double area { get; set; }
        public double thickness { get; set; }
        public int numberOfBrick { get; set; }
    }

    public static class Utils
    {
        public static double ftToMilimeters(double value)
        {
            return value * 100 / 0.32808398950131235;
        }

        public static double ft2ToMeters2(double value)
        {
            return 0.092903 * value;
        }

        public static double ft3ToMeters3(double value)
        {
            return 0.02832 * value;
        }
    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class Class1 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            ICollection<ElementId> ids = uidoc.Selection.GetElementIds();
            Document doc = uidoc.Document;
            List<double> areaList = new List<double>();
          
            double totalArea;
            //get form
            //form1.ShowDialog();
            //get value from form

            List<WallParams> data = new List<WallParams>();
            foreach (ElementId elemId in ids)
            {
                Element elem = doc.GetElement(elemId);
                Wall wall = elem as Wall;
                if (wall != null)
                {
                    WallParams pr = new WallParams();
                    pr.area = Utils.ft2ToMeters2(findDoublePrams(wall, "Area"));
                    pr.volume = Utils.ft3ToMeters3(findDoublePrams(wall, "Volume"));
                    pr.id = elem.Id.IntegerValue;

                    CompoundStructure compoundStructure = wall.WallType.GetCompoundStructure();
                    if (compoundStructure != null)
                    {
                        IList<CompoundStructureLayer> layers = compoundStructure.GetLayers();
                        for (int i = 0; i < layers.Count; i++)
                        {
                            if (layers[i].Function == MaterialFunctionAssignment.Structure)
                            {
                                var thickness = Utils.ftToMilimeters(layers[i].Width);
                                pr.thickness = thickness;
                            }
                        }
                        data.Add(pr);
                    }
                }
            }
            totalArea = areaList.Sum();
            double Soluonggach = totalArea / (200 * 55);
            // | ID | THE TICH | |
            Form1 form1 = new Form1(commandData);
            form1.setDataGrid(data);
            form1.ShowDialog();
            return Result.Succeeded;
        }
        public double findDoublePrams(Element elem, string paramName)
        {
            try
            {
                var value = elem.LookupParameter(paramName).AsDouble();
                return value;
            }
            catch
            {
                return 0.0;
            }
        }
        
    


    }
}
