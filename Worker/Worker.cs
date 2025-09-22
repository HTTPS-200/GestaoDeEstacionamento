using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Emgu.CV;
using Emgu.CV.Dnn;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Tesseract;
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Collections.Generic;

internal class Worker
{
    private static void Main(string[] args)
    {
        string yoloCfg = @"yolov4-placa.cfg";
        string yoloWeights = @"yolov4-placa.weights";
        Net net = null;
        if (File.Exists(yoloCfg) && File.Exists(yoloWeights))
        {
            net = DnnInvoke.ReadNetFromDarknet(yoloCfg, yoloWeights);
            net.SetPreferableBackend(Emgu.CV.Dnn.Backend.OpenCV);
            net.SetPreferableTarget(Emgu.CV.Dnn.Target.Cpu);
        }

        TesseractEngine ocr = null;
        if (Directory.Exists("tessdata"))
        {
            try { ocr = new TesseractEngine("./tessdata", "eng", EngineMode.Default); }
            catch { ocr = null; }
        }

        string ultimaPlaca = "";

        using var camera = new VideoCapture(0);
        if (!camera.IsOpened)
            return;

        new Thread(() =>
        {
            using var frame = new Mat();
            while (true)
            {
                if (camera.Read(frame) && !frame.IsEmpty)
                {
                    if (net != null && ocr != null)
                        ProcessarFrame(frame, net, ocr, ref ultimaPlaca);
                    else
                        ultimaPlaca = "[Câmera ativa]";
                }
                Thread.Sleep(100);
            }
        })
        { IsBackground = true }.Start();

        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();
        app.MapGet("/placa", () => ultimaPlaca);
        app.Run("http://localhost:5001");
    }

    private static void ProcessarFrame(Mat frame, Net net, TesseractEngine ocr, ref string ultimaPlaca)
    {
        using var resized = new Mat();
        CvInvoke.Resize(frame, resized, new Size(416, 416));
        using var blob = DnnInvoke.BlobFromImage(resized, 1 / 255.0, new Size(416, 416));
        net.SetInput(blob);
        using var output = new VectorOfMat();
        net.Forward(output, net.UnconnectedOutLayersNames);

        for (int i = 0; i < output.Size; i++)
        {
            using var mat = output[i];
            var detections = ProcessarSaidaYolo(mat, frame);
            foreach (var detection in detections)
            {
                if (detection.Confidence > 0.5f)
                {
                    var textoPlaca = ExecutarOCR(frame, detection.Rectangle, ocr);
                    if (!string.IsNullOrEmpty(textoPlaca))
                        ultimaPlaca = textoPlaca;
                }
            }
        }
    }

    private static List<Detecção> ProcessarSaidaYolo(Mat mat, Mat frameOriginal)
    {
        var detections = new List<Detecção>();
        var data = mat.GetData();
        for (int i = 0; i < mat.Rows; i++)
        {
            var confidence = Convert.ToSingle(data.GetValue(i, 4));
            if (confidence > 0.5f)
            {
                detections.Add(new Detecção
                {
                    Confidence = confidence,
                    Rectangle = ConverterCoordenadasYolo(data, i, frameOriginal.Width, frameOriginal.Height)
                });
            }
        }
        return detections;
    }

    private static Rectangle ConverterCoordenadasYolo(Array data, int index, int imgWidth, int imgHeight)
    {
        float centerX = Convert.ToSingle(data.GetValue(index, 0)) * imgWidth;
        float centerY = Convert.ToSingle(data.GetValue(index, 1)) * imgHeight;
        float width = Convert.ToSingle(data.GetValue(index, 2)) * imgWidth;
        float height = Convert.ToSingle(data.GetValue(index, 3)) * imgHeight;
        int x = (int)(centerX - width / 2);
        int y = (int)(centerY - height / 2);
        return new Rectangle(x, y, (int)width, (int)height);
    }

    private static string ExecutarOCR(Mat frame, Rectangle rect, TesseractEngine ocr)
    {
        try
        {
            using var roi = new Mat(frame, rect);
            using var bitmap = roi.ToBitmap();
            var tempFile = Path.GetTempFileName() + ".bmp";
            bitmap.Save(tempFile, System.Drawing.Imaging.ImageFormat.Bmp);
            using var pix = Pix.LoadFromFile(tempFile);
            using var page = ocr.Process(pix);
            File.Delete(tempFile);
            return page.GetText().Trim();
        }
        catch { return string.Empty; }
    }
}

public class Detecção
{
    public float Confidence { get; set; }
    public Rectangle Rectangle { get; set; }
}

public static class MatExtensions
{
    public static Bitmap ToBitmap(this Mat mat)
    {
        using var image = mat.ToImage<Bgr, byte>();
        return image.ToBitmap();
    }
}
