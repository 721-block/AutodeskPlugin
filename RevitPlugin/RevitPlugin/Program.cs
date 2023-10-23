using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;

namespace RevitPlugin
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Program : IExternalCommand
    {
        public Result Execute(ExternalCommandData externalCommandData, ref string message, ElementSet elements)
        {
            var document = externalCommandData.Application.ActiveUIDocument;
            var options = new Options();
            var select = new List<Element>();

            var appartmentReference = document.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Face);
            var appartment = document.Document.GetElement(appartmentReference);
            var apartmentGeometry = appartment.GetGeometryObjectFromReference(appartmentReference) as Face;
            var ap = apartmentGeometry.GetEdgesAsCurveLoops();

            var entranceReference = document.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Edge);
            var entrance = document.Document.GetElement(entranceReference);
            var entranceGeometry = entrance.GetGeometryObjectFromReference(entranceReference) as Edge;

            var balconyReference = document.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Edge);
            var balcony = document.Document.GetElement(balconyReference.ElementId);
            var balconyGeometry = balcony.GetGeometryObjectFromReference(balconyReference) as Edge;
            var edge1 = balconyGeometry.AsCurve();
            var edge = balconyGeometry.AsCurve().GetEndPoint(0);
            var x1 = balconyGeometry.AsCurve().GetEndPoint(1);

            var roomsWindow = new Rooms(appartment, entrance, balcony);
            roomsWindow.Show();
            return Result.Succeeded;
        }
    }
}
