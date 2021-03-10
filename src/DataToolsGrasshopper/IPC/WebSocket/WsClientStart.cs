//https://github.com/behrooz-tahanzadeh/Bengesht/blob/master/Bengesht/WsClient/WsClientStart.cs
using Grasshopper.Kernel;
using Grasshopper;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace DataToolsGrasshopper.IPC.WebSocket
{
    public class WsClientStart : GHIPCComponent
    {
        private WsObject wscObj;
        private bool isSubscribedToEvents;
        private GH_Document ghDocument;
        private WsAddress wsAddress;

        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public WsClientStart()
          : base("WebSocketClientStart", "WsStart",
              "Start WebSocket Client")
        {
            this.isSubscribedToEvents = false;
            this.wsAddress = new WsAddress("");
        }

        ~WsClientStart()
        {
            this.disconnect();
        }


        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("address", "URL", "Websocket server address. Scheme (ws://) should be included. For example <b>ws://echo.websocket.org</b>", GH_ParamAccess.item);
            pManager.AddTextParameter("inital message", "Msg", "initial message", GH_ParamAccess.item, "Hello World");
            pManager.AddBooleanParameter("reset", "Rst", "Restart the connection.", GH_ParamAccess.item, false);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Websocket Objects", "WSC", "This object provides access to the connection. Connect this output to WS input websocket Send/Recv components.", GH_ParamAccess.item);
        }

        /// <summary>
        /// Disconnect from websocket server.
        /// This function needs to be run on events such as delete the component.
        /// </summary>
        private void disconnect()
        {
            if (this.wscObj != null)
            {
                try { this.wscObj.disconnect(); }
                catch { }
                this.wscObj.changed -= this.wsObjectOnChange;
                this.wscObj = null;
                this.wsAddress.setAddress(null);
            }
        }

        /// <summary>
        /// Detecting the deletation of this component and run disconnect function.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void documentOnObjectsDeleted(object sender, GH_DocObjectEventArgs e)
        {
            if (e.Objects.Contains(this))
            {
                e.Document.ObjectsDeleted -= documentOnObjectsDeleted;
                this.disconnect();
            }
        }

        private void documentServerOnDocumentClosed(GH_DocumentServer sender, GH_Document doc)
        {
            if (this.ghDocument != null && doc.DocumentID == this.ghDocument.DocumentID)
            {
                this.disconnect();
            }
        }

        void onObjectChanged(IGH_DocumentObject sender, GH_ObjectChangedEventArgs e)
        {
            if (this.Locked)
                this.disconnect();
        }

        private void subscribeToEvents()
        {
            if (!this.isSubscribedToEvents)
            {
                this.ghDocument = OnPingDocument();

                if (this.ghDocument != null)
                {
                    this.ghDocument.ObjectsDeleted += documentOnObjectsDeleted;
                    Instances.DocumentServer.DocumentRemoved += documentServerOnDocumentClosed;
                }

                this.ObjectChanged += this.onObjectChanged;
                this.isSubscribedToEvents = true;
            }
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            this.subscribeToEvents();

            string address = null;
            string initMsg = "Hello World";
            bool reset = false;

            DA.GetData(0, ref address);
            if (!DA.GetData(1, ref initMsg)) return;
            if (!DA.GetData(2, ref reset)) return;

            if (!this.wsAddress.isSameAs(address) || reset)
            {
                this.disconnect();

                this.wsAddress.setAddress(address);

                if (this.wsAddress.isValid())
                {
                    this.wscObj = new WsObject().init(address, initMsg);
                    this.Message = "Connecting";
                    this.wscObj.changed += this.wsObjectOnChange;
                }
                else
                {
                    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid address");
                }
            }

            DA.SetData(0, this.wscObj);
        }

        private void wsObjectOnChange(object sender, EventArgs e)
        {
            this.Message = WsObjectStatus.GetStatusName(this.wscObj.status);
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
            get { return new Guid("72bb5cdd-66a1-4b5c-adab-dc773a6df7d0"); }
        }
    }
}