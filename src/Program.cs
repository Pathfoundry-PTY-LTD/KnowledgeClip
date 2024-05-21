using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Newtonsoft.Json.Linq;

namespace KnowledgeClip
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // Make sure this application runs as a Windows application without a console window
            if (!AttachConsole(-1))
            {
                FreeConsole();
            }

            // Run the KnowledgeClip logic
            RunKnowledgeClip(args);
        }

        private static void RunKnowledgeClip(string[] args)
        {
            if (args.Length != 1)
            {
                ShowMessageBox("Usage: KnowledgeClip <directoryPath>", "Error", 0x00000010);
                return;
            }

            string directoryPath = args[0];

            // Read file extensions from config.json
            string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
            if (!File.Exists(configFilePath))
            {
                ShowMessageBox("Configuration file 'config.json' not found.", "Error", 0x00000010);
                return;
            }

            string[] extensions;
            try
            {
                var config = JObject.Parse(File.ReadAllText(configFilePath));
                extensions = config["fileExtensions"]?.ToObject<string[]>();
                if (extensions == null || extensions.Length == 0)
                {
                    ShowMessageBox("No file extensions found in configuration file.", "Error", 0x00000010);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Error reading configuration file: {ex.Message}", "Error", 0x00000010);
                return;
            }

            try
            {
                // Verify that the directory exists
                if (!Directory.Exists(directoryPath))
                {
                    ShowMessageBox($"The directory '{directoryPath}' does not exist.", "Error", 0x00000010);
                    return;
                }

                // Get all files with the specified extensions in the directory and subdirectories
                var files = Directory.EnumerateFiles(directoryPath, "*.*", SearchOption.AllDirectories)
                    .Where(file => extensions.Contains(Path.GetExtension(file).ToLower()))
                    .ToList();

                // Combine the contents of the files into a single string
                StringBuilder combinedContent = new StringBuilder();

                combinedContent.AppendLine("===== START OF FILE DUMPS =====");
                foreach (var file in files)
                {
                    string relativePath = Path.GetRelativePath(directoryPath, file);
                    combinedContent.AppendLine($"===== START OF FILE: {relativePath} =====");
                    combinedContent.AppendLine(File.ReadAllText(file));
                    combinedContent.AppendLine($"===== END OF FILE: {relativePath} =====");
                }
                combinedContent.AppendLine("===== END OF FILE DUMPS =====");

                // Copy the combined content to the clipboard
                combinedContent.ToString().CopyToClipboard();

                ShowMessageBox("The combined content has been copied to the clipboard.", "Success", 0x00000040);
            }
            catch (Exception ex)
            {
                ShowMessageBox($"An error occurred: {ex.Message}", "Error", 0x00000010);
            }
        }

        private static void ShowMessageBox(string text, string caption, uint type)
        {
            MessageBox(IntPtr.Zero, text, caption, type);
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int MessageBox(IntPtr hWnd, string lpText, string lpCaption, uint uType);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AttachConsole(int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool FreeConsole();
    }
}