//https://github.com/behrooz-tahanzadeh/Bengesht/blob/master/Bengesht/WsClient/WsClientSend.cs
using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace DataToolsGrasshopper.IPC.WebSocket
{
    public class WsClientSend : GHIPCComponent
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public WsClientSend()
          : base("MyComponent1", "Nickname",
              "Description")
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
            get { return new Guid("b2caa429-4454-4b99-bd51-ea083c218b2c"); }
        }
    }
}