//https://github.com/behrooz-tahanzadeh/Bengesht/blob/master/Bengesht/WsClient/WsClientRecv.cs
using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;

namespace DataToolsGrasshopper.IPC.WebSocket
{
    public class WsClientReceive : GHIPCComponent
    {
        private WsObject wscObj;
        private bool onMessageTriggered;
        private GH_Document ghDocument;
        private bool isAutoUpdate;
        private bool isAskingNewSolution;
        private List<string> buffer = new List<string>();
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public WsClientReceive()
          : base("WsClientReceive", "WsRecv",
              "Display Message Received from WebSocket Server")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Websocket Objects", "WSC", "websocket objects", GH_ParamAccess.item);
            pManager.AddTextParameter("Message", "Msg", "Message content", GH_ParamAccess.item, "Hello World");
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            WsObject wscObj = new WsObject();
            string message = "Hello World";

            if (!DA.GetData(0, ref wscObj)) return;
            if (!DA.GetData(1, ref message)) return;

            wscObj.send(message);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("e91df9f9-7846-440e-91e5-798acd4d2736"); }
        }
    }
}