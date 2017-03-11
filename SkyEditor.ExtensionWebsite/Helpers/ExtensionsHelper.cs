using Microsoft.EntityFrameworkCore;
using SkyEditor.Core.Extensions;
using SkyEditor.ExtensionWebsite.Data;
using SkyEditor.ExtensionWebsite.Models.ExtensionModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace SkyEditor.ExtensionWebsite.Helpers
{
    public static class ExtensionsHelper
    {
        /// <summary>
        /// Creates or selects the default extension collection.
        /// </summary>
        /// <param name="context">The data current data context from which to select the collection.</param>
        /// <returns>The default extension collection.</returns>
        public static async Task<ExtensionCollection> GetDefaultExtensionCollectionAsync(ExtensionsDbContext context, string collectionName)
        {
            var collection = await context.ExtensionCollections.Where(x => x.ParentID.HasValue && x.Name == collectionName).FirstOrDefaultAsync();
            if (collection == null)
            {
                collection = new ExtensionCollection { ID = Guid.NewGuid(), Name = collectionName };
                context.ExtensionCollections.Add(collection);
                await context.SaveChangesAsync();
            }
            return collection;
        }

        /// <summary>
        /// Installs an extension to the database and filesystem.
        /// </summary>
        /// <param name="filename">Path of the extension zip file.</param>
        /// <param name="collectionID">ID of the collection in which to put the extension.  Must be "linked" to entity framework.</param>
        /// <param name="context">The data current data context in which to put the extension metadata.</param>
        public static async Task InstallExtension(string filename, Guid collectionID, ExtensionsDbContext context, ExtensionsConfig config)
        {
            // Read the info file
            string infoContents = null;
            using (var zip = ZipFile.OpenRead(filename))
            {
                var entry = zip.GetEntry("info.skyext");
                using (var entryStream = entry.Open())
                {
                    using (var reader = new StreamReader(entryStream))
                    {
                        infoContents = await reader.ReadToEndAsync();
                    }
                }
            }

            if (!string.IsNullOrEmpty(infoContents))
            {
                var infoFile = ExtensionInfo.Deserialize(infoContents);

                // Create or select the extension
                var extension = await context.Extensions.Where(x => x.CollectionID == collectionID && x.ExtensionID == infoFile.ID).FirstOrDefaultAsync();
                if (extension == null)
                {
                    extension = new Extension { ID = Guid.NewGuid(), ExtensionID = infoFile.ID, CollectionID = collectionID, Name = infoFile.Name, Description = infoFile.Description, Author = infoFile.Author };
                    context.Extensions.Add(extension);
                }

                // Create or select version info
                var version = await context.ExtensionVersions.Where(x => x.ExtensionID == extension.ID && x.Version == infoFile.Version).FirstOrDefaultAsync();
                if (version == null)
                {
                    version = new ExtensionVersion { ID = Guid.NewGuid(), Version = infoFile.Version, ExtensionID = extension.ID };
                    context.ExtensionVersions.Add(version);
                }

                await context.SaveChangesAsync();

                // Ensure target directory exists
                var extensionDir = Path.Combine(config.ExtensionsPath, "files", extension.ExtensionID);
                if (!Directory.Exists(extensionDir))
                {
                    Directory.CreateDirectory(extensionDir);
                }

                // Copy the file
                File.Copy(filename, Path.Combine(extensionDir, version.Version + ".zip"));
            }
            else
            {
                throw new ArgumentException("Provided zip file does not contain a valid info file.", nameof(filename));
            }
        }

        /// <summary>
        /// Installs an extension to the database and filesystem.
        /// </summary>
        /// <param name="filename">Path of the extension zip file.</param>
        public static async Task InstallExtension(string filename, ExtensionsConfig config)
        {
            using (var context = ExtensionsDbContext.Create())
            {
                var collection = await GetDefaultExtensionCollectionAsync(context, config.DefaultCollectionName);
                await InstallExtension(filename, collection.ID, context, config);
            }                
        }
    }
}
