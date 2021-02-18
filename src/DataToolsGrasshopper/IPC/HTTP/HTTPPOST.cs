using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;

using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

using DataToolsGrasshopper.Utils;


namespace DataToolsGrasshopper.IPC.HTTP
{
    public class HTTPPOST : GHIPCComponent
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public HTTPPOST() : base("HTTP-POST",
              "IPC", "Handles HTTP POST request")
        {

        }
        public override GH_Exposure Exposure => GH_Exposure.primary;
        protected override System.Drawing.Bitmap Icon => Properties.Resources.icons_http_post;

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("url", "U", "URL for HTTP server.", GH_ParamAccess.item);
            pManager.AddIntegerParameter("timeout", "T", "Time out for HTTP GET request", GH_ParamAccess.item);
            pManager.AddTextParameter("JSONContent", "J", "JSON Content to Send?", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>  
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("response", "R", "HTTP response", GH_ParamAccess.item);

        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess access)
        {
            string url = "";
            access.GetData(0, ref url);

            int timeout = 5000;
            access.GetData(1, ref timeout);

            string JSONcocntent = "";
            access.GetData(2, ref JSONcocntent);

            if (url == null) return;

            if (timeout == 0) timeout = 5000;

            // From https://stackoverflow.com/a/4015346/1934487

            var request = (HttpWebRequest)WebRequest.Create(url);

            request.Timeout = timeout;

            // Parameters can be added to the query URLs
            //var postData = "thing1=" + Uri.EscapeDataString("hello");
            //postData += "&thing2=" + Uri.EscapeDataString("world");

            var data = Encoding.ASCII.GetBytes(JSONcocntent);

            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var res = (HttpWebResponse)request.GetResponse();

            var responseString = new StreamReader(res.GetResponseStream()).ReadToEnd();

            access.SetData(0, responseString);
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("27262774-f087-4378-9004-d2efe024de20"); }
        }
    }
}