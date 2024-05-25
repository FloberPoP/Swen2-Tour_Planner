using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System.IO;
using Tour_Planner.Models;

public class ReportGenerator
{
    public void GeneratePdf(string filePath, Tour tour)
    {
        // Create a new Document
        Document document = new Document();

        try
        {
            // Initialize a PdfWriter instance to write the document to the file
            PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

            // Open the Document
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

            // Add a line separator
            LineSeparator line = new LineSeparator(1f, 100f, BaseColor.BLACK, Element.ALIGN_CENTER, -1);
            document.Add(line);

            // Add any additional content as needed
            // Example: Add tour logs, map images, etc.

        }
        catch (DocumentException ex)
        {
            // Handle DocumentException, if any
            Console.WriteLine($"An error occurred while generating PDF: {ex.Message}");
        }
        finally
        {
            // Close the Document
            document.Close();
        }
    }
}
