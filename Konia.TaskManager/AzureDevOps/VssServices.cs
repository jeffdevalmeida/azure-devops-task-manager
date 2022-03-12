using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Client;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

namespace Konia.TaskManager.AzureDevOps
{
    internal class VssServices
    {
        private static string c_collectionUri = ConfigurationManager.AppSettings.Get("AzureDevOps_Collection");
        private static string c_projectName = ConfigurationManager.AppSettings.Get("AzureDevOps_TeamProject");

        private const string wi_type_task = "Task";

        private static VssConnection connection;

        internal static Microsoft.VisualStudio.Services.Identity.Identity PromptCredentials()
        {
            connection = new VssConnection(new Uri(c_collectionUri), new VssClientCredentials());
            return connection.AuthenticatedIdentity;
        }

        internal static async Task InsertTask(Model.Task task)
        {
            PromptCredentials();

            if (connection is null)
                throw new ArgumentNullException(nameof(connection));

            using (WorkItemTrackingHttpClient wiClient = connection.GetClient<WorkItemTrackingHttpClient>())
            {
                WorkItem parentRequirement = await wiClient.GetWorkItemAsync(task.RequirementId);

                IDictionary<string, object> dict = ConstructDictionaryPath(task, parentRequirement);
                var jsonPatchDocument = VssJsonPatchDocumentFactory.ConstructJsonPatchDocument(Microsoft.VisualStudio.Services.WebApi.Patch.Operation.Add, dict);
                WorkItem workItem = await wiClient.CreateWorkItemAsync(jsonPatchDocument, c_projectName, wi_type_task);
            }
        }

        private static IDictionary<string, object> ConstructDictionaryPath(Model.Task task, WorkItem parent)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>
            {
                { "fields/System.Title", task.Title },
                { "fields/Microsoft.VSTS.Scheduling.OriginalEstimate", task.Hours },
                { "fields/Microsoft.VSTS.Scheduling.RemainingWork", task.Hours },
                { "fields/System.AssignedTo", connection.AuthenticatedIdentity.Properties["Account"].ToString() },
                {
                    "relations/-",
                    new
                    {
                        rel = "System.LinkTypes.Hierarchy-Reverse",
                        url = parent.Url
                    }
                }
            };

            return dict;
        }
    }
}
