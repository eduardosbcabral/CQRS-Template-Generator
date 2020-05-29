using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CQRSTempGen
{
    class Program
    {
        static readonly string APPLICATION_PATH = "C:\\Users\\Eduardo\\source\\repos\\CQRSTempGen";
        static readonly string TEMPLATE_PATH = Path.Combine(APPLICATION_PATH, "Templates");
        static readonly string OUTPUT_PATH = Path.Combine(APPLICATION_PATH, "Output");

        static void Main(string[] args)
        {
            if (Directory.Exists(OUTPUT_PATH))
                Directory.Delete(OUTPUT_PATH, true);
            else
                Directory.CreateDirectory(OUTPUT_PATH);

            Console.WriteLine("Solution folder: ");
            var solutionFolder = Console.ReadLine();

            while (!Directory.Exists(solutionFolder))
            {
                Console.WriteLine("Solution folder does not exists, type again: ");
                solutionFolder = Console.ReadLine();
            }

            while(!Directory.GetFiles(solutionFolder, "*.sln").Any())
            {
                Console.WriteLine("Solution folder does not contains a .sln file, type again: ");
                solutionFolder = Console.ReadLine();
            }

            Console.WriteLine("Entity name: ");
            var entityName = Console.ReadLine();

            Console.WriteLine("Database table name: ");
            var tableDatabaseEntity = Console.ReadLine();

            Console.WriteLine("Copy files to solution? (y/n): ");
            var copyFilesToSolution = Console.ReadLine();

            while (copyFilesToSolution != "y" && copyFilesToSolution != "n")
            {
                Console.WriteLine("Invalid anwser. Copy files to solution? (y/n): ");
                copyFilesToSolution = Console.ReadLine();
            }

            var coreProjectName = Directory.GetDirectories(solutionFolder, "*.Core")
                .FirstOrDefault()
                .Split("\\")
                .LastOrDefault();

            var solutionProjectName = coreProjectName.Split(".")[0];

            foreach(var template in GetTemplateFiles())
            {
                var text = File.ReadAllText(Path.Combine(TEMPLATE_PATH, $"{template.Item2}.txt"));
                text = text
                    .Replace("%SolutionName%", solutionProjectName)
                    .Replace("%CoreProjectName%", coreProjectName)
                    .Replace("%EntityName%", entityName)
                    .Replace("%EntityNameLowerCase%", FirstCharToLowerCase(entityName))
                    .Replace("%EntityNameTableDatabase%", tableDatabaseEntity);

                var outputFolder = Path.Combine(OUTPUT_PATH, template.Item1);

                if (!Directory.Exists(outputFolder))
                    Directory.CreateDirectory(outputFolder);

                if (copyFilesToSolution == "n")
                {
                    var outputFile = Path.Combine(
                    OUTPUT_PATH,
                    template.Item1,
                    template.Item2.Replace("EntityName", entityName) + template.Item3);

                    File.WriteAllText(outputFile, text);
                    Console.WriteLine("Project files generated in output folder!");

                }
                else
                {
                    var outputSolutionFileFolder = Path.Combine(
                        solutionFolder,
                        $"{solutionProjectName}.{template.Item1.Replace("EntityName", entityName)}");

                    if (!Directory.Exists(outputSolutionFileFolder))
                        Directory.CreateDirectory(outputSolutionFileFolder);

                    File.WriteAllText(Path.Combine(
                        outputSolutionFileFolder, 
                        template.Item2.Replace("EntityName", entityName) + template.Item3),
                    text);

                    Console.WriteLine("Project files copied to solution folder!");
                }
            }

        }

        static string BuildEntityNameDatabaseTable(string input)
        {
            input = FirstCharToLowerCase(input);

            var table = new StringBuilder("TB_");

            var previousChar = char.MinValue;

            foreach (var c in input)
            {
                if (char.IsUpper(c))
                {
                    if (table.Length != 0 && previousChar != ' ')
                    {
                        table
                            .Append("_")
                            .Append(c);
                    }
                }
                else
                {
                    table.Append(char.ToUpper(c));
                }

                previousChar = c;
            }

            return table
                .Append("S")
                .ToString();
        }

        static string FirstCharToLowerCase(string input) =>
            input switch
            {
                null => throw new ArgumentNullException(nameof(input)),
                "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
                _ => input.First().ToString().ToLower() + input.Substring(1)
            };

        static List<Tuple<string, string, string>> GetTemplateFiles() =>
            new List<Tuple<string, string, string>>
            {
                new Tuple<string, string, string>("Core\\Application\\Services", "EntityNameAppService", ".cs"),
                new Tuple<string, string, string>("Core\\Application\\Interfaces", "IEntityNameAppService", ".cs"),
                new Tuple<string, string, string>("Core\\Application\\ViewModels\\EntityName", "EntityNameViewModel", ".cs"),
                new Tuple<string, string, string>("Core\\Domain\\CommandHandlers", "EntityNameCommandHandler", ".cs"),
                new Tuple<string, string, string>("Core\\Domain\\Commands\\EntityName", "EntityNameCommand", ".cs"),
                new Tuple<string, string, string>("Core\\Domain\\Commands\\EntityName", "RegisterEntityNameCommand", ".cs"),
                new Tuple<string, string, string>("Core\\Domain\\Commands\\EntityName", "RemoveEntityNameCommand", ".cs"),
                new Tuple<string, string, string>("Core\\Domain\\Commands\\EntityName", "UpdateEntityNameCommand", ".cs"),
                new Tuple<string, string, string>("Core\\Domain\\Entities", "EntityName", ".cs"),
                new Tuple<string, string, string>("Core\\Domain\\EventHandlers", "EntityNameEventHandler", ".cs"),
                new Tuple<string, string, string>("Core\\Domain\\Events", "EntityNameRegisteredEvent", ".cs"),
                new Tuple<string, string, string>("Core\\Domain\\Events", "EntityNameRemovedEvent", ".cs"),
                new Tuple<string, string, string>("Core\\Domain\\Events", "EntityNameUpdatedEvent", ".cs"),
                new Tuple<string, string, string>("Core\\Domain\\Interfaces", "IEntityNameRepository", ".cs"),
                new Tuple<string, string, string>("Core\\Domain\\Queries", "EntityNameQueries", ".cs"),
                new Tuple<string, string, string>("Core\\Domain\\Validations", "EntityNameValidation", ".cs"),
                new Tuple<string, string, string>("Core\\Domain\\Validations", "RegisterEntityNameCommandValidation", ".cs"),
                new Tuple<string, string, string>("Core\\Domain\\Validations", "RemoveEntityNameCommandValidation", ".cs"),
                new Tuple<string, string, string>("Core\\Domain\\Validations", "UpdateEntityNameCommandValidation", ".cs"),
                new Tuple<string, string, string>("Core\\Infrastructure\\Config\\Maps", "EntityNameMap", ".cs"),
                new Tuple<string, string, string>("Core\\Infrastructure\\Repositories", "EntityNameRepository", ".cs"),
                new Tuple<string, string, string>("Web\\Controllers", "EntityNameController", ".cs"),
                new Tuple<string, string, string>("Web\\Views\\EntityName", "Create", ".cshtml"),
                new Tuple<string, string, string>("Web\\Views\\EntityName", "Delete", ".cshtml"),
                new Tuple<string, string, string>("Web\\Views\\EntityName", "Edit", ".cshtml"),
                new Tuple<string, string, string>("Web\\Views\\EntityName", "Index", ".cshtml"),
            };
    }
}
