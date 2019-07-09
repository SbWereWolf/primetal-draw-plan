using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Xml;

namespace draw_plan
{
    class Program
    {
        private static void Main(string[] args)
        {
            var files = new string[0];
            var letGather = false;
            if (args?.Length != 0)
            {
                files = args;
                letGather = true;
            }
            if (!letGather)
            {
                Console.WriteLine("have no detect file to Draw");
            }

            var letDraw = false;
            var designs = new List<Outline>();
            if (letGather)
            {
                designs = Gather(files);
                letDraw = designs?.Count > 0;
            }

            if (letDraw)
            {
                Draw(designs);
            }
        }

        private static List<Outline> Gather(string[] files)
        {
            var designs = new List<Outline>();

            if (files != null)
                foreach (var file in files)
                {
                    var tryExecute = File.Exists(file);

                    var outlines = new List<Outline>();
                    if (tryExecute && file != null)
                    {
                        outlines = GetDesigns(file);
                    }

                    if (outlines != null && outlines.Count > 0)
                    {
                        designs.AddRange(outlines);
                    }
                }
            return designs;
        }

        private static void Draw(List<Outline> designs)
        {
            if (designs != null)
                foreach (var outline in designs)
                {
                    var dimensions = outline?.GetDimensions();
                    if (dimensions != null)
                    {
                        dimensions.Extends(outline.Margin * 2);
                        var bitmap = new Bitmap(dimensions.Width, dimensions.Length, PixelFormat.Format32bppArgb);
                        var canvas = Graphics.FromImage(bitmap);

                        var body = outline.Body;
                        var pen = new Pen(Color.Blue);
                        body?.Contour?.Draw(canvas, pen, outline.Margin);

                        pen = new Pen(Color.Red);
                        // ReSharper disable PossibleNullReferenceException
                        foreach (var aperture in outline.Next())
                        // ReSharper restore PossibleNullReferenceException
                        {
                            aperture?.Contour?.Draw(canvas, pen, outline.Margin);
                        }

                        bitmap.Save($"{outline.Name}.png", ImageFormat.Png);
                    }
                }
        }


        private static List<Outline> GetDesigns(string file)
        {
            var designs = new List<Outline>();
            using (var reader = XmlReader.Create(file ?? throw new ArgumentNullException(nameof(file))))
            {
                var contour = new Contour(0, 0);
                var body = new Shape(contour);
                var apertures = new List<Shape>();
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        int x;
                        int y;
                        switch (reader.Name)
                        {
                            case "contour":
                                x = 0;
                                y = 0;
                                var startX = reader["startX"];
                                if (startX != null)
                                {
                                    x = int.Parse(startX);
                                }
                                var startY = reader["startY"];
                                if (startY != null)
                                {
                                    y = int.Parse(startY);
                                }
                                contour = new Contour(x, y);
                                break;
                            case "vector":
                                x = 0;
                                y = 0;
                                var finishX = reader["X"];
                                if (finishX != null)
                                {
                                    x = int.Parse(finishX);
                                }
                                var finishY = reader["Y"];
                                if (finishY != null)
                                {
                                    y = int.Parse(finishY);
                                }
                                contour.AddVector(new Vector(x, y));
                                break;
                        }
                    }
                    if (reader.NodeType == XmlNodeType.EndElement)
                    {
                        switch (reader.Name)
                        {
                            case "outline":
                                var name = Path.GetFileNameWithoutExtension(file);
                                var outline = new Outline(body, name);
                                foreach (var shape in apertures)
                                {
                                    outline.AddShape(shape);
                                }
                                designs.Add(outline);
                                break;
                            case "body":
                                body = new Shape(contour);
                                break;
                            case "aperture":
                                apertures.Add(new Shape(contour));
                                break;
                        }
                    }
                }
            }

            return designs;
        }
    }
}
