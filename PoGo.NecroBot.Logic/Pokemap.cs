using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PoGo.NecroBot.Logic
{
    public partial class Pokemap : Form
    {
        private static Pokemap pokemap = null;
        public enum MarkerType { Character, Pokeball }
        private Dictionary<MarkerType, string> markerPath = new Dictionary<MarkerType, string>();
        GMapOverlay oldMarkerOverlay = null;
        GMarkerGoogle oldMarker = null;

        public static Pokemap GetInstance()
        {
            if (pokemap == null)
            {
                pokemap = new Pokemap();
            }
            return pokemap;
        }

        private Pokemap()
        {
            Console.WriteLine(Directory.GetCurrentDirectory());
            Console.WriteLine("C:\\Users\\adrie\\Source\\Bot\\NecroBot\\PoGo.NecroBot.Logic\\Images");

            this.markerPath.Add(MarkerType.Character, "..\\..\\..\\PoGo.NecroBot.Logic\\Images\\pokemon.gif");
            this.markerPath.Add(MarkerType.Pokeball, "..\\..\\..\\PoGo.NecroBot.Logic\\Images\\pokeball.png");

            InitializeComponent();

            this.Load += Map2D_Load;
        }

        private void Map2D_Load(object sender, EventArgs e)
        {
            this.gMapControl1.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerOnly;
            this.gMapControl1.ShowCenter = false;
        }

        private string loadMarkerFromImages(string name)
        {
            var outPutDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            var logoimage = Path.Combine(outPutDirectory, name);
            return new Uri(logoimage).LocalPath;
        }

        private void PutMarker(double lat, double lng, MarkerType type)
        {
            GMapOverlay markersOverlay = new GMapOverlay("markers");
            GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(lat, lng), new Bitmap(loadMarkerFromImages(this.markerPath[type])));

            this.gMapControl1.Overlays.Add(markersOverlay);
            markersOverlay.Markers.Add(marker);

            if (type == MarkerType.Character)
            {
                oldMarker = marker;
                oldMarkerOverlay = markersOverlay;
                this.gMapControl1.Position = new PointLatLng(lat, lng);
            }
        }

        private void ChooseMarker(double lat, double lng, MarkerType type)
        {
            if (oldMarkerOverlay != null && oldMarker != null && type == MarkerType.Character)
            {
                oldMarkerOverlay.Markers.Remove(oldMarker);
            }

            try
            {
                PutMarker(lat, lng, type);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
            }
        }

        public void UpdatePosition(double lat, double lng, MarkerType type)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.ChooseMarker(lat, lng, type);
        }
    }
}
