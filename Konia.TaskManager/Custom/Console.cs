using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konia.TaskManager.Custom
{
    internal static class Console
    {
        public static T ReadLine<T>() where T : struct, IConvertible
        {
            T obj;

            while (true)
            {
                System.Console.WriteLine(">> ");
                string value = System.Console.ReadLine();

                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                    System.Console.WriteLine("Por favor, insira um valor não vazio");
                else
                {
                    try
                    {
                        obj = (T)Convert.ChangeType(value, typeof(T));
                        break;
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine();
                        System.Console.WriteLine(">> Ocorreu um erro durante o processamento: {0}", ex.Message);
                        System.Console.WriteLine();

                        System.Console.WriteLine(">> Por favor, tente inserir novamente o valor solicitado");
                        continue;
                    }
                }
            }

            return obj;
        }

        public static string ReadLine()
        {
            string obj;

            while (true)
            {
                System.Console.WriteLine(">> ");
                string value = System.Console.ReadLine();

                if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                    System.Console.WriteLine("Por favor, insira um valor não vazio.");
                else
                {
                    obj = value;
                    break;
                }
            }

            return obj;
        }
    }
}
