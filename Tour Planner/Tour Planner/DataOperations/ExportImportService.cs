using System;
using System.Collections.ObjectModel;
using System.IO;
using log4net;
using Newtonsoft.Json;
using Tour_Planner.BL;
using Tour_Planner.DAL;
using Tour_Planner.Models;

public class ExportImportService
{
    private static readonly ILog Log = LogManager.GetLogger(typeof(App));

    public static void ExportTourToFile(Tour tour)
    {
        string documentsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        string fileName = $"Tour_{timeStamp}.json";
        string filePath = Path.Combine(documentsDirectory, fileName);

        try
        {
            // Create JsonSerializerSettings with ReferenceLoopHandling.Ignore
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            // Serialize the tour to JSON with the specified settings
            string json = JsonConvert.SerializeObject(tour, Formatting.Indented, settings);

            // Write the JSON to the file
            File.WriteAllText(filePath, json);

           Log.Info($"Tour exported successfully to: {filePath}");
        }
        catch (Exception ex)
        {
            Log.Info($"An error occurred while exporting tour to file: {ex.Message}");
        }
    }


    public static Tour ImportTourFromFile(string filePath, List<Tour> tours)
    {
        try
        {
            // Read the JSON from the file
            string json = File.ReadAllText(filePath);

            // Deserialize the JSON to a Tour object
            Tour importedTour = JsonConvert.DeserializeObject<Tour>(json);

            foreach (var tour in tours)
            {
                if (tour.Id == importedTour.Id)
                {
                    Log.Info($"Same Tour with ID {importedTour.Id} already exists. Skipping import.");
                    return null;
                }
            }

            // Check for existing TourLogs with the same ID
            foreach (var tourLog in importedTour.TourLogs)
            {
                foreach (var tour in tours)
                {
                    foreach (var existingTourLog in tour.TourLogs)
                    {
                        if (existingTourLog.Id == tourLog.Id)
                        {
                            Log.Info($"Same TourLog with ID {tourLog.Id} already exists. Skipping import.");
                            return null;
                        }
                    }
                }
            }

            return importedTour;
        }
        catch (Exception ex)
        {
            Log.Info($"An error occurred while importing tour from file: {ex.Message}");
            return null;
        }
    }
}