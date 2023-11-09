using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Collections.Generic;
using System.Linq;

namespace RevitPlugin
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Program : IExternalCommand
    {
        public Result Execute(ExternalCommandData externalCommandData, ref string message, ElementSet elements)
        {
            var document = externalCommandData.Application.ActiveUIDocument;
            //TaskDialog.Show("Выбор места для квартиры", "Выберите место, где хотите сгенерировать квартиру");
            var appartmentReference = document.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Face);
            var appartment = document.Document.GetElement(appartmentReference);
            var apartmentGeometry = appartment.GetGeometryObjectFromReference(appartmentReference) as Face;
            var walls = apartmentGeometry.GetEdgesAsCurveLoops()[0];

            //TaskDialog.Show("Выбор стены с балконом", "Выберите стену, где будет находиться балкон");
            var balconyReference = document.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Edge);
            var balcony = document.Document.GetElement(balconyReference.ElementId);
            var balconyGeometry = balcony.GetGeometryObjectFromReference(balconyReference) as Edge;
            var balconyWall = balconyGeometry.AsCurve();

            //TaskDialog.Show("Выбор стены со входом", "Выберите стену, где будет находиться вход");
            var entranceReference = document.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Edge);
            var entrance = document.Document.GetElement(entranceReference);
            var entranceGeometry = entrance.GetGeometryObjectFromReference(entranceReference) as Edge;
            var entranceWall = entranceGeometry.AsCurve();

            var roomsWindow = new Rooms(balconyWall, entranceWall, walls, document.Document);
            roomsWindow.Show();
            return Result.Succeeded;
        }
    }
}
