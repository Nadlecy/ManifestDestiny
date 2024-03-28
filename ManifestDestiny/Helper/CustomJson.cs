using System;
using System.IO;
using System.Text.Json;

namespace ManifestDestiny
{
    public class CustomJson<T>
    {
        private string _filePath;

        public CustomJson(string filePath)
        {
            _filePath = "../../../Data/" + filePath;
        }

        public T Read()
        {
            try
            {
                // Vérifier si le fichier existe
                if (File.Exists(_filePath))
                {

                    // Lire le contenu du fichier JSON
                    string jsonContent = File.ReadAllText(_filePath);
                    // Désérialiser le contenu JSON en objet de type T
                    T? data = JsonSerializer.Deserialize<T>(jsonContent);

                    return data;
                }
                else
                {
                    //Console.WriteLine(_filePath + ": Le fichier JSON n'existe pas.");
                    throw new Exception(_filePath + ": Le fichier JSON n'existe pas.");
                    //return default(T);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de la lecture du fichier JSON : {ex.Message}");
                return default(T);
            }
        }

        public void Write(T content)
        {

        }
    }
}
