using iTextSharp.text;
using iTextSharp.text.pdf;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tour_Planner.DAL;
using Tour_Planner.Models;

public class ReportGenerator
{
    private static readonly ILog Log = LogManager.GetLogger(typeof(App));
    public static void GenerateTourReport(Tour tour)
    {
        Document document = new Document();

        try
        {
            string appDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string reportFileName = $"report_{tour.Name}_{timestamp}.pdf";
            string filePath = Path.Combine(appDataDirectory, reportFileName);
            PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
            document.Open();

            // Add Title
            Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, BaseColor.BLACK);
            Paragraph title = new Paragraph($"Tour Report for {tour.Name}", titleFont);
            title.Alignment = Element.ALIGN_CENTER;
            title.SpacingAfter = 20f;
            document.Add(title);

            // Add Tour Details
            Font detailsFont = FontFactory.GetFont(FontFactory.HELVETICA, 12, BaseColor.BLACK);
            document.Add(new Paragraph($"Description: {tour.Description}", detailsFont));
            document.Add(new Paragraph($"From: {tour.From}", detailsFont));
            document.Add(new Paragraph($"To: {tour.To}", detailsFont));
            document.Add(new Paragraph($"Transport Type: {tour.TransportType}", detailsFont));
            document.Add(new Paragraph($"Distance: {tour.Distance} km", detailsFont));
            document.Add(new Paragraph($"Estimated Time: {tour.EstimatedTime} hours", detailsFont));

            // Add Tour Logs
            if (tour.TourLogs != null && tour.TourLogs.Any())
            {
                document.Add(new Paragraph("Tour Logs:", detailsFont));
                foreach (var log in tour.TourLogs)
                {
                    document.Add(new Paragraph($"Date: {log.DateTime}, Comment: {log.Comment}, Difficulty: {log.Difficulty}, Distance: {log.TotalDistance}, Time: {log.TotalTime}, Rating: {log.Rating}", detailsFont));
                }
            }
            else
            {
                document.Add(new Paragraph("No tour logs available.", detailsFont));
            }
        }
        catch (DocumentException ex)
        {
            Log.Info($"Dowload Exception TourReport:  {ex.Message}");
            Console.WriteLine($"An error occurred while generating PDF: {ex.Message}");
        }
        finally
        {
            Log.Info($"Dowload Exception TourReport:  Worked");
            document.Close();
        }
    }

    public static void GenerateSummarizeReport(Tour tour)
    {
        Document document = new Document();

        try
        {
            string appDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string reportFileName = $"report_{tour.Name}_{timestamp}.pdf";
            string filePath = Path.Combine(appDataDirectory, reportFileName);
            PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));
            document.Open();

            // Add Title
            Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, BaseColor.BLACK);
            Paragraph title = new Paragraph($"Summarize Report for {tour.Name}", titleFont);
            title.Alignment = Element.ALIGN_CENTER;
            title.SpacingAfter = 20f;
            document.Add(title);

            // Calculate average time, distance, and rating
            if (tour.TourLogs != null && tour.TourLogs.Any())
            {
                var avgTime = tour.TourLogs.Average(log => TimeSpan.Parse(log.TotalTime).TotalHours);
                var avgDistance = tour.TourLogs.Average(log => double.Parse(log.TotalDistance));
                var avgRating = tour.TourLogs.Average(log => log.Rating);

                // Add average information to the report
                document.Add(new Paragraph($"Average Time: {avgTime} hours", titleFont));
                document.Add(new Paragraph($"Average Distance: {avgDistance} km", titleFont));
                document.Add(new Paragraph($"Average Rating: {avgRating}", titleFont));
            }
            else
            {
                document.Add(new Paragraph("No tour logs available.", titleFont));
            }
        }
        catch (DocumentException ex)
        {
            Log.Info($"Dowload Exception SummarizedTourReport:  {ex.Message}");
            Console.WriteLine($"An error occurred while generating PDF: {ex.Message}");
        }
        finally
        {
            Log.Info($"Dowload Exception SummarizedTourReport:  Worked");
            document.Close();
        }
    }
}
