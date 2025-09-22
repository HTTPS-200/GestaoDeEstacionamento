using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using System.Drawing;
using System.Threading;
using Emgu.CV;
using Emgu.CV.Dnn;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Tesseract;
using System;

internal class Program
{
    private static void Main(string[] args)
    {
        // Caminhos dos modelos YOLO
        string yoloCfg = @"yolov4-placa.cfg";
        string yoloWeights = @"yolov4-placa.weights";

        // Inicializa YOLO
        var net = DnnInvoke.ReadNetFromDarknet(yoloCfg, yoloWeights);
        net.SetPreferableBackend(Emgu.CV.Dnn.Backend.Default);
        net.SetPreferableTarget(Emgu.CV.Dnn.Target.Cpu);

        string ultimaPlaca = "";


        using var camera = new VideoCapture(0);
        using var frame = new Mat();

        using var ocr = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default);


        _ = new Thread(() =>
        {
            while (true)
            {
                camera.Read(frame);
                if (!frame.IsEmpty)
                {

                    using var blob = DnnInvoke.BlobFromImage(frame, 1 / 255.0, new Size(416, 416), new MCvScalar(), true, false);
                    net.SetInput(blob);

                    var model = new DetectionModel(net);
                    model.SetInputParams(1 / 255.0, new Size(416, 416), new MCvScalar(), true);
                    var boxes = new VectorOfRect();
                    var confidences = new VectorOfFloat();
                    var classIds = new VectorOfInt();

                    model.Detect(frame, boxes, classIds, confidences, 0.5f);

                    for (int i = 0; i < boxes.Size; i++)
                    {
                        Rectangle rect = boxes[i];
                        using var roi = new Mat(frame, rect);

                        // Converte Mat para Bitmap para Tesseract
                        using var bitmap = roi.ToBitmap();
                        using var pix = PixConverter.ToPix(bitmap);

                        var resultado = ocr.Process(pix);
                        ultimaPlaca = resultado.GetText().Trim();
                    }
                }
                Thread.Sleep(50);
            }
        }).Start();


        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        app.MapGet("/placa", () => ultimaPlaca);

        app.Run("http://localhost:5001");
    }
}
