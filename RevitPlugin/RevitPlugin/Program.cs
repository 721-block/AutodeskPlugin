﻿using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;

namespace RevitPlugin
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Program : IExternalCommand
    {
        static AddInId addinId = new AddInId(new Guid("0330CEFC-2D00-4679-8575-415900518532"));
        
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

            var parameters = new ApartmentParameters(balconyWall, entranceWall, walls, document.Document);
            parameters.ShowDialog();
            return Result.Succeeded;
        }
    }
}