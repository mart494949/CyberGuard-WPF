using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using cyberbezpieczny.Models;

namespace cyberbezpieczny.Services
{
    public class FileManager
    {
        // Ładuje moduł z pliku JSON
        public Module LoadModule(string filePath)
        {
            try
            {
                string jsonContent = File.ReadAllText(filePath);
                Module module = JsonConvert.DeserializeObject<Module>(jsonContent);
                return module;
            } 
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Error: Module file not found. {ex.Message}");
                return null;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error loading module: {ex.Message}");
                return null;
            }


        }

        // Ładuje postępy gracza z pliku JSON
        public Player LoadProgress(string filePath)
        {
            try
            {
                if(!File.Exists(filePath))
                {
                    return new Player();
                }

                string jsonContent = File.ReadAllText(filePath);
                Player player = JsonConvert.DeserializeObject<Player>(jsonContent);
                return player;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading progress: {ex.Message}");
                return new Player();
            }
        }

        // Zapisuje postępy gracza do pliku JSON

        public void SaveProgress(Player player, string filePath)
        {
            try
            {
                string jsonContent = JsonConvert.SerializeObject(player, Formatting.Indented);
                File.WriteAllText(filePath, jsonContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving progress: {ex.Message}");
            }
        }

    }
}
