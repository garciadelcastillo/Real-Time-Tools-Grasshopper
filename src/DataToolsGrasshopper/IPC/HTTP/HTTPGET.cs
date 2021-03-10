using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

using DataToolsGrasshopper.Utils;

namespace DataToolsGrasshopper.IPC.HTTP
{
    public class HTTPGET : GHIPCComponent
    {

        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public HTTPGET() : base(
                "HTTP-GET",
              "IPC","Handles HTTP GET request")
        {
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;
        protected override System.Drawing.Bitmap Icon => Properties.Resources.icons_http_get;

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("url", "U", "URL for HTTP server.", GH_ParamAccess.item);
            pManager.AddIntegerParameter("timeout", "T", "Time out for HTTP GET request", GH_ParamAccess.item);
            pManager.AddBooleanParameter("send", "B", "Send Request?", GH_ParamAccess.item, false);
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

            bool send = false;
            access.GetData(2, ref send);

            if (url == null || !send) return;

            if (timeout == 0) timeout = 5000;

            System.Net.ServicePointManager.Expect100Continue = true;
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12; //the auth type

            var request = System.Net.WebRequest.Create(url);
            request.Timeout = timeout;

            // If required by the server, set the credentials.
            request.Credentials = System.Net.CredentialCache.DefaultCredentials;

            // Get the response.
            var res = (System.Net.HttpWebResponse)request.GetResponse();

            // Display the status.
            Console.WriteLine(res.StatusDescription);

            // Get the stream containing content returned by the server.
            Stream dataStream = res.GetResponseStream();

            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);

            // Read the content.
            var response = reader.ReadToEnd();

            access.SetData(0, response);

            reader.Close();
            dataStream.Close();
            res.Close();
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("1d7f66fe-60ab-4df6-89fc-c462863f0171"); }
        }
    }
}