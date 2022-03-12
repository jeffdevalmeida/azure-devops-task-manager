using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Konia.TaskManager
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            ConsoleColor defaultColor = Console.ForegroundColor;
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            Culture.SetCulture();

            Console.WriteLine("|{0,10} {1,13} {0,13}|", string.Empty, "Bem-vindo");
            Console.WriteLine("|{0,10} {1,16} {0,10}|", string.Empty, "Criação de Tasks");

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine(">> Aguarde enquanto te autenticamos...");
            var credentials = AzureDevOps.VssServices.PromptCredentials();

            if (credentials != null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ocorreu algum erro durante a autenticação. Por favor tente novamente mais tarde.");
                Console.ForegroundColor = defaultColor;
                Environment.Exit(1);
            }

            Console.WriteLine(">> ({0}) Usuário autenticado com sucesso.", credentials.DisplayName);

            while (true)
            {
                Console.WriteLine();

                // Get the task informations for creation.
                Console.WriteLine(">> Por favor, insira as informações abaixo.");
                Console.WriteLine();

                Model.Task task = new Model.Task();

                // Get idenfier of task.
                Console.WriteLine(">> Digite o identificador do requisito como pai da tarefa.");
                task.RequirementId = Custom.Console.ReadLine<int>();

                // Get the display title of task.
                Console.WriteLine(">> Digite de forma resumida, o descritivo da tarefa.");
                task.Title = Custom.Console.ReadLine();

                // Get the hours of task.
                Console.WriteLine(">> Por fim, informe a quantidade de horas para alocação da atividade.");
                task.Hours = Custom.Console.ReadLine<double>();

                // Insert task on Azure DevOps.
                await Services.TaskServices.InsertTask(task);
            }
        }
    }
}
