namespace KnowledgeClip;

using System;
using System.IO;
using System.Linq;
using System.Text;

class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        // Check if a directory path is provided
        if (args.Length != 1)
        {
            Console.WriteLine("Usage: KnowledgeClip <directoryPath>");
            return;
        }

        string directoryPath = args[0];
        
        // Define the file extensions to include
        string[] extensions = { ".txt", ".cs", ".html" }; // Add or modify extensions as needed

        try
        {
            // Verify that the directory exists
            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine($"The directory '{directoryPath}' does not exist.");
                return;
            }

            // Get all files with the specified extensions in the directory and subdirectories
            var files = Directory.EnumerateFiles(directoryPath, "*.*", SearchOption.AllDirectories)
                .Where(file => extensions.Contains(Path.GetExtension(file).ToLower()))
                .ToList();

            // Combine the contents of the files into a single string
            StringBuilder combinedContent = new StringBuilder();

            foreach (var file in files)
            {
                combinedContent.AppendLine(File.ReadAllText(file));
            }

            // Copy the combined content to the clipboard using the extension method
            combinedContent.ToString().CopyToClipboard();

            Console.WriteLine("The combined content has been copied to the clipboard.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}

