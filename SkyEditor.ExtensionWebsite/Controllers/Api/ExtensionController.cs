using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkyEditor.Core.Extensions.Online;
using SkyEditor.ExtensionWebsite.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SkyEditor.ExtensionWebsite.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/Extension")]
    public class ExtensionController : Controller
    {
        public ExtensionController(IOptions<ExtensionsConfig> options, ExtensionsDbContext context)
        {
            this.Options = options.Value;
            this.context = context;
        }
        protected ExtensionsDbContext context;
        protected ExtensionsConfig Options { get; set; }

        // GET: api/Extension
        [Route("api/Extensions/{collectionId}/{skip}/{take}")]
        public async Task<IEnumerable<OnlineExtensionInfo>> Get(Guid collectionID, int skip, int take)
        {
            return await (from e in context.Extensions
                          let v = e.Versions.Select(x => x.Version)
                          where e.CollectionID == collectionID
                          orderby e.Name
                          select new OnlineExtensionInfo
                          {
                              ID = e.ExtensionID,
                              Name = e.Name,
                              Description = e.Description,
                              Author = e.Author,
                              AvailableVersions = v.ToList()
                          }).Skip(skip).Take(take).ToListAsync();
        }

        // GET: api/Extension/5
        [Route("api/Extensions/{extensionID}/{version}.zip")]
        public ActionResult Get(string extensionID, string version)
        {
            var filename = Path.Combine(Options.ExtensionsPath, "files", extensionID, version + ".zip");
            if (System.IO.File.Exists(filename))
            {
                return PhysicalFile(filename, "application/sky-editor-extension", $"{extensionID} {version}.zip");
            }
            else
            {
                return this.StatusCode(404);
            }
        }

        //' GET: api/Extensions/extensionName/1.0.0
        //<Route("api/Extensions/{extensionID}/{version}.zip")>
        //Public Function GetValue(extensionID As String, version As String) As HttpResponseMessage
        //    Dim filename = IO.Path.Combine(ConfigurationManager.AppSettings("ExtensionsPath"), "files", extensionID, version & ".zip")
        //    If IO.File.Exists(filename) Then
        //        Dim streamContent = New StreamContent(IO.File.OpenRead(filename))
        //        streamContent = streamContent
        //        streamContent.Headers.ContentType = New Headers.MediaTypeHeaderValue("application/sky-editor-extension")

        //        Dim response = Request.CreateResponse
        //        response.Content = streamContent
        //        Return response
        //    Else
        //        Return Request.CreateResponse(404)
        //    End If
        //End Function

        //// POST: api/Extension
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/Extension/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/ApiWithActions/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
