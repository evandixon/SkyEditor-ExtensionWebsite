using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkyEditor.ExtensionWebsite.Models.ExtensionModels;
using SkyEditor.ExtensionWebsite.Data;
using SkyEditor.Core.Extensions.Online;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SkyEditor.ExtensionWebsite.Helpers;

namespace SkyEditor.ExtensionWebsite.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/ExtensionCollection")]
    public class ExtensionCollectionController : Controller
    {

        public ExtensionCollectionController(IOptions<ExtensionsConfig> options, ExtensionsDbContext context)
        {
            this.Options = options.Value;
            this.context = context;
        }

        protected ExtensionsDbContext context;

        protected ExtensionsConfig Options { get; set; }



        // GET: api/ExtensionCollection
        [HttpGet]
        public async Task<RootCollectionResponse> Get()
        {
            var collection = await ExtensionsHelper.GetDefaultExtensionCollectionAsync(context, Options.DefaultCollectionName);
            return await GetRootExtensionCollectionResponse(collection, context);
        }

        // GET: api/ExtensionCollection/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<RootCollectionResponse> Get(Guid id)
        {
            var collection = await context.ExtensionCollections.Where(x => x.ID == id).FirstOrDefaultAsync();
            return await GetRootExtensionCollectionResponse(collection, context);
        }

        private async Task<RootCollectionResponse> GetRootExtensionCollectionResponse(ExtensionCollection collection, ExtensionsDbContext context)
        {
            var output = new RootCollectionResponse();
            output.Name = collection.Name;
            output.ChildCollections = await (from c in context.ExtensionCollections
                                             where c.ParentID == collection.ID
                                             select new ExtensionCollectionModel { ID = c.ID.ToString(), Name = c.Name }).ToListAsync();

            output.DownloadExtensionEndpoint = Options.APIRoot + "Extensions";
            output.GetExtensionListEndpoint = Options.APIRoot + "Extensions/" + collection.ID.ToString();
            output.ExtensionCount = await context.Extensions.Where(x => x.CollectionID == collection.ID).CountAsync();
            return output;
        }

        //// POST: api/ExtensionCollection
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/ExtensionCollection/5
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
