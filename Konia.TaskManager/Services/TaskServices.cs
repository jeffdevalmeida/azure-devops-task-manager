using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Konia.TaskManager.Services
{
    internal static class TaskServices
    {
        [Range(1, int.MaxValue)]
        private static readonly int _defaultTaskQuantityHour = 8;

        public static async Task InsertTasks(IEnumerable<Model.Task> tasks)
        {
            foreach (var task in tasks)
                await InsertTask(task);
        }

        public static async Task InsertTask(Model.Task task)
        {
            int quantityToCreate = (int)Math.Ceiling(task.Hours / _defaultTaskQuantityHour);

            for (int i = 0; i < quantityToCreate; i++)
            {
                Model.Task separatedTask = new Model.Task($"{task.Title} {i + 1}", task.RequirementId);

                double currentHours = task.Hours;
                task.Hours -= _defaultTaskQuantityHour;

                if (task.Hours >= 0)
                    separatedTask.Hours = _defaultTaskQuantityHour;
                else
                    separatedTask.Hours = currentHours;

                await AzureDevOps.VssServices.InsertTask(separatedTask);
            }
        }
    }
}
