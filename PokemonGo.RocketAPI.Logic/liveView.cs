using PokemonGo.RocketAPI.GeneratedCode;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;

namespace PokemonGo.RocketAPI.Logic
{
    public partial class liveView : Form
    {
        private static ImageList _imagesList;
        private static DateTime _startDateTime = DateTime.Now;
        private static long _startExperience = 0;
        private static long _currentExperience = 0;
        private static int _bagSpace = 0;
        private static int _pokemonSpace = 0;
        private static Timer _exphrUpdater;
        private static Dictionary<string, GMapOverlay> _mapOverlays;

        public liveView()
        {
            InitializeComponent();

            _imagesList = new ImageList();
            _imagesList.ImageSize = new Size(40, 30);
            LoadImages();

            gMap.MapProvider = GoogleMapProvider.Instance;
            GMaps.Instance.Mode = AccessMode.ServerOnly;
            gMap.Zoom = 15;

            _mapOverlays = new Dictionary<string, GMapOverlay>();
            _mapOverlays.Add("pokemons", new GMapOverlay("pokemons"));
            gMap.Overlays.Add(_mapOverlays["pokemons"]);

            _mapOverlays.Add("pokestops", new GMapOverlay("pokestops"));
            gMap.Overlays.Add(_mapOverlays["pokestops"]);

            _mapOverlays.Add("pokegyms", new GMapOverlay("pokegyms"));
            gMap.Overlays.Add(_mapOverlays["pokegyms"]);

            _mapOverlays.Add("avatar", new GMapOverlay("avatar"));
            gMap.Overlays.Add(_mapOverlays["avatar"]);

            Bitmap avatar = new Bitmap(_imagesList.Images[_imagesList.Images.IndexOfKey("avatar")]);
            avatar.MakeTransparent(Color.White);
            GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(0, 0), avatar);
            _mapOverlays["avatar"].Markers.Add(marker);

            //Bitmap bmp = new Bitmap(pokeImageList.Images[1]);
            //bmp.MakeTransparent(Color.White);
            //GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(45.5056207, -73.6128037), bmp);
            //marker.Size = new Size(40, 30);
            //marker.ToolTipText = "Test\n\r2";
            //marker.Tag = "1";
            ////marker.IsVisible = false;
            //pokemonOverlay.Markers.Add(marker);

            //PointLatLng center = new PointLatLng(45.5056207, -73.6128037);
            //for(int x=1;x<=50;x++)
            //{
            //    marker = new GMarkerGoogle(new PointLatLng(center.Lat, center.Lng), bmp);
            //    scanOverlay.Markers.Add(marker);
            //    center.Lat = center.Lat + (x * 0.00001);
            //}

            _exphrUpdater = new Timer();
            _exphrUpdater.Interval = 1000;
            _exphrUpdater.Tick += new EventHandler(UpdateExpHr);
            _exphrUpdater.Start();
        }
        
        private void LoadImages()
        {
            var resourceImages = Resource.ResourceManager.GetResourceSet(CultureInfo.InvariantCulture, true, false);
            foreach (DictionaryEntry entry in resourceImages)
            {
                var value = entry.Value as Bitmap;
                if (value != null)
                {
                    _imagesList.Images.Add((string)entry.Key, value);
                }
             }
         }

        public void UpdateLatLng(double lat, double lng)
        {
            gMap.Invoke(new Action(() => gMap.Position = new PointLatLng(lat, lng)));
            textCurrentLatLng.Invoke(new Action(() => textCurrentLatLng.Text = lat.ToString() + "," + lng.ToString()));
            _mapOverlays["avatar"].Markers[0].Position = new PointLatLng(lat, lng);
        }

        public void UpdateMapPokeStops(IEnumerable<FortData> pokestopsOnMap)
        {
            var currentListPokestop = _mapOverlays["pokestops"].Markers.ToList();

            foreach (var line in currentListPokestop)
            {
                if (pokestopsOnMap.Where(p => p.Id.ToString() == (string)line.Tag).ToList().Count == 0)
                {
                    gMap.Invoke(new Action(() => _mapOverlays["pokestops"].Markers.Remove(line)));
                }
            }

            Bitmap pokestopImg = new Bitmap(_imagesList.Images[_imagesList.Images.IndexOfKey("pokestop")],new Size(20,20));
            pokestopImg.MakeTransparent(Color.White);

            Bitmap pokestopluredImg = new Bitmap(_imagesList.Images[_imagesList.Images.IndexOfKey("pokestop_lured")], new Size(20, 20));
            pokestopluredImg.MakeTransparent(Color.White);

            foreach (var pokestop in pokestopsOnMap)
            {
                if (currentListPokestop.Where(p => (string)p.Tag == pokestop.Id.ToString()).Count() == 0)
                {
                    GMarkerGoogle marker;
                    if (pokestop.LureInfo != null)
                    {
                        marker = new GMarkerGoogle(new PointLatLng(pokestop.Latitude, pokestop.Longitude), pokestopluredImg);
                    }
                    else
                    {
                        marker = new GMarkerGoogle(new PointLatLng(pokestop.Latitude, pokestop.Longitude), pokestopImg);
                    }
                    marker.Tag = pokestop.Id.ToString();
                    gMap.Invoke(new Action(() => _mapOverlays["pokestops"].Markers.Add(marker)));
                }
            }
            textTotalPokestop.Invoke(new Action(() => textTotalPokestop.Text = _mapOverlays["pokestops"].Markers.Count.ToString()));
        }

        public void UpdateMapPokemons(IEnumerable<MapPokemon> pokemonsOnMap)
        {
            var currentListPokemons = _mapOverlays["pokemons"].Markers.ToList();

            foreach (var line in currentListPokemons)
            {
                if (pokemonsOnMap.Where(p => p.EncounterId.ToString() == (string)line.Tag).ToList().Count == 0)
                {
                    gMap.Invoke(new Action(() => _mapOverlays["pokemons"].Markers.Remove(line)));
                }
            }

            foreach (var pokemon in pokemonsOnMap)
            {
                if (currentListPokemons.Where(p => (string)p.Tag == pokemon.EncounterId.ToString()).Count() == 0)
                {

                    Bitmap pokemonImg = new Bitmap(_imagesList.Images[_imagesList.Images.IndexOfKey("pokemon_" + ((int)pokemon.PokemonId).ToString())], new Size(40, 30));
                    pokemonImg.MakeTransparent(Color.White);

                    GMarkerGoogle marker = new GMarkerGoogle(new PointLatLng(pokemon.Latitude, pokemon.Longitude), pokemonImg);
                    marker.Tag = pokemon.EncounterId.ToString();
                    marker.ToolTipText = pokemon.PokemonId.ToString();
                    gMap.Invoke(new Action(() => _mapOverlays["pokemons"].Markers.Add(marker)));
                }
            }
            textTotalPokemons.Invoke(new Action(() => textTotalPokemons.Text = _mapOverlays["pokemons"].Markers.Count.ToString()));
        }

        public void UpdateMyPokemons(IEnumerable<PokemonData> mypokemons)
        {
            var currentList = dataMyPokemons.Rows.OfType<DataGridViewRow>().ToArray();

            foreach (var line in currentList)
            {
                if (mypokemons.Where(p => p.Id.ToString() == (string)line.Cells[3].Value).ToList().Count == 0)
                {
                    dataMyPokemons.Invoke(new Action(() => dataMyPokemons.Rows.Remove(line)));
                }
            }

            foreach (var pokemon in mypokemons)
            {
                if (currentList.Where(p => (string)p.Cells[3].Value == pokemon.Id.ToString()).Count() == 0)
                {
                    if (_imagesList.Images.ContainsKey("pokemon_" + ((int)pokemon.PokemonId).ToString()))
                        dataMyPokemons.Invoke(new Action(() => dataMyPokemons.Rows.Add(_imagesList.Images[_imagesList.Images.IndexOfKey("pokemon_" + ((int)pokemon.PokemonId).ToString())], pokemon.PokemonId.ToString(), pokemon.Cp, pokemon.Id.ToString())));
                    else
                        dataMyPokemons.Invoke(new Action(() => dataMyPokemons.Rows.Add(new Bitmap(40, 30), pokemon.PokemonId.ToString(), pokemon.Cp, pokemon.Id.ToString())));
                }
            }

            dataMyPokemons.Invoke(new Action(() => dataMyPokemons.Sort(dataMyPokemons.Columns[1], ListSortDirection.Ascending)));
            labelPokemonSpace.Invoke(new Action(() => labelPokemonSpace.Text = mypokemons.Count().ToString() + "/" + _bagSpace.ToString()));
        }

        public void UpdateMyItems(IEnumerable<Item> myitems)
        {
            int total = 0;

            var currentList = dataMyItems.Rows.OfType<DataGridViewRow>().ToArray();

            foreach(var line in currentList)
            {
                if (myitems.Where(i => i.Item_.ToString() == (string)line.Cells[3].Value).ToList().Count == 0)
                {
                    dataMyItems.Invoke(new Action(() => dataMyItems.Rows.Remove(line)));
                }
            }

            foreach (var item in myitems)
            {
                if (currentList.Where(p => (string)p.Cells[3].Value == item.Item_.ToString()).Count() == 0)
                {
                    object name = Enum.Parse(typeof(ItemId), ((int)item.Item_).ToString());
                    if (_imagesList.Images.ContainsKey("item_" + ((int)item.Item_).ToString()))
                        dataMyItems.Invoke(new Action(() => dataMyItems.Rows.Add(_imagesList.Images[_imagesList.Images.IndexOfKey("item_" + ((int)item.Item_).ToString())], name, item.Count.ToString(), item.Item_.ToString())));
                    else
                        dataMyItems.Invoke(new Action(() => dataMyItems.Rows.Add(new Bitmap(40, 30), name, item.Count.ToString(), item.Item_.ToString())));
                }
                else
                {
                    DataGridViewRow row = currentList.Where(p => (string)p.Cells[3].Value == item.Item_.ToString()).FirstOrDefault();
                    if (row != null)
                        dataMyItems.Invoke(new Action(() => dataMyItems[2, row.Index].Value = item.Count));
                }
                total += item.Count;
            }
            labelBagSpace.Invoke(new Action(() => labelBagSpace.Text = total.ToString() + "/" +_bagSpace.ToString()));
        }

        public void UpdateMyStats(PlayerStats mystats)
        {
            if (_startExperience == 0)
                _startExperience = mystats.Experience;

            textLevel.Invoke(new Action(() => textLevel.Text = mystats.Level.ToString()));
            progressLevel.Invoke(new Action(() => progressLevel.Minimum = 0));
            progressLevel.Invoke(new Action(() => progressLevel.Maximum = (int)mystats.NextLevelXp));
            progressLevel.Invoke(new Action(() => progressLevel.Value = (int)mystats.Experience));
            labelExp.Invoke(new Action(() => labelExp.Text = mystats.Experience.ToString() + "/" + mystats.NextLevelXp.ToString()));

            _currentExperience = mystats.Experience;
        }

        public void UpdateMyProfile(Profile myprofile)
        {
            textPokecoins.Invoke(new Action(() => textPokecoins.Text = myprofile.Currency[0].Amount.ToString()));
            textStardust.Invoke(new Action(() => textStardust.Text = myprofile.Currency[1].Amount.ToString()));

            _bagSpace = myprofile.ItemStorage;
            _pokemonSpace = myprofile.PokeStorage;
        }

        private void UpdateExpHr(Object myObject, EventArgs myEventArgs)
        {
            double exphr = Math.Round((_currentExperience - _startExperience) / (DateTime.Now - _startDateTime).TotalHours);
            labelExpHr.Invoke(new Action(() => labelExpHr.Text = exphr.ToString() + " XP/HR"));
        }

        private void liveView_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            return;
        }
    }
}
