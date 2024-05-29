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

            // Check if a tour with the same ID already exists
            Tour existingTour = TourExists(importedTour, tours);

            if (existingTour != null)
            {
                Log.Info($"Same Tour with ID {importedTour.Id} already exists. Skipping import.");
                return null;
            }
            else
            {
                // Tour can be imported, return it
                return importedTour;
            }
        }
        catch (Exception ex)
        {
            Log.Info($"An error occurred while importing tour from file: {ex.Message}");
            return null;
        }
    }
    private static int GenerateNewId()
    {
        return Guid.NewGuid().GetHashCode();
    }
    private static Tour TourExists(Tour importedTour, List<Tour> tours)
    {
        foreach (var tour in tours)
        {
            if (tour.Id == importedTour.Id)
            {
                if (AreToursEqual(importedTour, tour))
                {
                    // Same tour already exists, return null
                    return null;
                }
                else
                {
                    // Same ID but different data, generate new ID
                    importedTour.Id = GenerateNewId();
                    return importedTour;
                }
            }
        }

        // Tour with the same ID doesn't exist
        return null;
    }

    private static bool AreToursEqual(Tour tour1, Tour tour2)
    {        
        return tour1.Name == tour2.Name &&
               tour1.Description == tour2.Description &&
               tour1.From == tour2.From &&
               tour1.To == tour2.To &&
               tour1.TransportType == tour2.TransportType &&
               tour1.Distance == tour2.Distance &&
               tour1.EstimatedTime == tour2.EstimatedTime &&
               tour1.Img == tour2.Img;
    }
}