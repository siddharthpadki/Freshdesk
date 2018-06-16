using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Nito.AsyncEx;

namespace Freshdesk
{
    class Program
    {
        static void Main(string[] args)
        {
            AsyncContext.Run(() => MainAsync(args));
        }

        static async void MainAsync(string[] args)
        {
            FreshdeskApi api = new FreshdeskApi();

            await api.deleteAllCategories();

            string analyticsCategoryId = await api.createCategory("Analytics", "Analytics category.");
            string monitoringCategoryId = await api.createCategory("Monitoring", "Monitoring category.");
            string operationsCategoryId = await api.createCategory("Operations", "Operations category.");

            string configurationFolderId = await api.createFolder(analyticsCategoryId, "Configuration", "Configuration folder.", 1);
            string gettingStartedFolderId = await api.createFolder(analyticsCategoryId, "Getting Started", "Getting started in Analytics Folder", 1);

            string articleId = await api.createArticle(configurationFolderId, "how-to-configure-analytics", "How to configure analytics.", 1, 1);
            string analyticsArticleId = await api.createArticle(gettingStartedFolderId, "getting-started-with-analytics", "Getting started with Analytics", 1, 1);

            string monitoringConfigurationFolderId = await api.createFolder(monitoringCategoryId, "Configuration", "Configuration folder in monitoring", 1);
            string configurationHelpArticleId = await api.createArticle(monitoringConfigurationFolderId, "configuration-help", "Configuration Help", 1, 1);

            string monitoringGettingStartedFolderId = await api.createFolder(monitoringCategoryId, "Getting Started", "Getting started in monitoring folder", 1);
            string monitoringStartedArticleId = await api.createArticle(monitoringGettingStartedFolderId, "getting-started-with-monitoring", "Getting started with Monitoring", 1, 1);

            string operationsFolderId = await api.createFolder(operationsCategoryId, "getting-started-with-biztalk360", "Getting started with biztalk360", 1);
            string operationsArticleId = await api.createArticle(operationsFolderId, "getting-started-with-biztalk360", "This article explains how to get started with biztalk360", 1, 1);

            string documentationCategoryId = await api.createCategory("Documentation", "Documentation category");
            string documentationFolderId = await api.createFolder(documentationCategoryId, "getting-started-with-biztalk360", "Getting started with biztalk360", 1);
            string documentationArticleId = await api.createArticle(documentationFolderId, "getting-started-with-biztalk360", "This article explains how to get started with biztalk360. This is new addition to the existing article.", 1, 1);

            string newBarnCategoryId = await api.createCategory("New Barn Category", "This is a test category");
            Console.ReadKey();
        }

    }
}
