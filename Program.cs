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

                var outputFile = Path.Combine(
                    OUTPUT_PATH,
                    template.Item1,
                    template.Item2.Replace("EntityName", entityName) + template.Item3);

                File.WriteAllText(outputFile, text);
            }

            Console.WriteLine("Project files generated!");
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
                new Tuple<string, string, string>("Services", "EntityNameAppService", ".cs"),
                new Tuple<string, string, string>("Services", "IEntityNameAppService", ".cs"),
                new Tuple<string, string, string>("ViewModels", "EntityNameViewModel", ".cs"),
                new Tuple<string, string, string>("CommandHandlers", "EntityNameCommandHandler", ".cs"),
                new Tuple<string, string, string>("Commands", "EntityNameCommand", ".cs"),
                new Tuple<string, string, string>("Commands", "RegisterEntityNameCommand", ".cs"),
                new Tuple<string, string, string>("Commands", "RemoveEntityNameCommand", ".cs"),
                new Tuple<string, string, string>("Commands", "UpdateEntityNameCommand", ".cs"),
                new Tuple<string, string, string>("Entities", "EntityName", ".cs"),
                new Tuple<string, string, string>("EventHandlers", "EntityNameEventHandler", ".cs"),
                new Tuple<string, string, string>("Events", "EntityNameRegisteredEvent", ".cs"),
                new Tuple<string, string, string>("Events", "EntityNameRemovedEvent", ".cs"),
                new Tuple<string, string, string>("Events", "EntityNameUpdatedEvent", ".cs"),
                new Tuple<string, string, string>("Interfaces", "IEntityNameRepository", ".cs"),
                new Tuple<string, string, string>("Queries", "EntityNameQueries", ".cs"),
                new Tuple<string, string, string>("Validations", "EntityNameValidation", ".cs"),
                new Tuple<string, string, string>("Validations", "RegisterEntityNameCommandValidation", ".cs"),
                new Tuple<string, string, string>("Validations", "RemoveEntityNameCommandValidation", ".cs"),
                new Tuple<string, string, string>("Validations", "UpdateEntityNameCommandValidation", ".cs"),
                new Tuple<string, string, string>("Maps", "EntityNameMap", ".cs"),
                new Tuple<string, string, string>("Repositories", "EntityNameRepository", ".cs"),
                new Tuple<string, string, string>("Controllers", "EntityNameController", ".cs"),
                new Tuple<string, string, string>("Views", "Create", ".cshtml"),
                new Tuple<string, string, string>("Views", "Delete", ".cshtml"),
                new Tuple<string, string, string>("Views", "Edit", ".cshtml"),
                new Tuple<string, string, string>("Views", "Index", ".cshtml"),
            };
    }
}
