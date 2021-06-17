﻿using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using PdfGenerationHelper.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PdfGenerationHelper.Controls
{
    public class FilesControl
    {
        /// <summary>
        /// Add a text to the document
        /// </summary>
        /// <param name="path">String with image name.</param>
        public static Image CreateImage(string path)
        {
            ImageData imageData = ImageDataFactory.Create(path);
            Image image = new(imageData);
            return image;
        }

        /// <summary>
        /// Fill a table with the data received.
        /// </summary>
        /// <param name="objects">IEnumerable with the objects to complete the table.</param>
        /// <returns>Return a filled table.</returns>
        public static Table CreateTable<T>(IEnumerable<T> objects)
        {
            Type type = objects.First().GetType();

            IEnumerable<PropertyInfo> properties = type
                .GetProperties()
                .Where(p => p.PropertyType.Namespace == "System");

            Table table = new(properties.Count());

            IEnumerable<PdfObject<T>> ListPdfObjects = objects
                .Select(o => new PdfObject<T>
                {
                    OriginalObject = o,
                    Properties = properties.Select(p => new PdfObjectProperty
                    {
                        PropertyInfo = p,
                        Value = type
                                .GetProperty(p.Name)
                                .GetValue(o)?.ToString()
                    })
                });

            foreach (PropertyInfo property in properties)
            {
                table.AddHeaderCell(property.Name);
            }

            foreach (PdfObject<T> pdfObject in ListPdfObjects)
            {
                foreach (PdfObjectProperty property in pdfObject.Properties)
                {
                    table.AddCell(string.IsNullOrEmpty(property.Value) ? "" : property.Value);
                }
            }
            return table;
        }
    }
}
