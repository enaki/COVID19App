﻿using core;
using LiveCharts.Maps;
using LiveCharts.WinForms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Media;


namespace view
{
    /// <summary>
    /// Class responsible for creating the map from
    /// the country info list and notifying the subscribers
    /// about click events.
    /// </summary>
    public class MapView : IView
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="info">
        /// The country information to represent on the map
        /// </param>
        public MapView (IReadOnlyList<CountryInfoEx> info)
        {
            _countries = info;

            _map.Source = _MapFile;
            _map.Dock = DockStyle.Fill;

            GradientStopCollection collection = new GradientStopCollection();
            collection.Add(new GradientStop(Colors.Green, 0.0));
            collection.Add(new GradientStop(Colors.Yellow, 0.5));
            collection.Add(new GradientStop(Colors.Red, 1.0));

            _map.GradientStopCollection = collection;

            CountryInfoEx mostSevere = Utils.MaxElement(info,
                (CountryInfoEx c1, CountryInfoEx c2) => 
                    (c1.Confirmed - c1.Recovered - c1.Deaths) > (c2.Confirmed - c2.Recovered - c2.Deaths));

            double half = Math.Log((mostSevere.Confirmed - mostSevere.Recovered - mostSevere.Deaths) / 2.0);

            Dictionary<string, double> scaledValues = new Dictionary<string, double>();
            foreach (CountryInfoEx country in info)
            {
                int active = country.Confirmed - country.Deaths - country.Recovered;
                double confirmed = (active > 0) ? Math.Log(active) : 0;

                scaledValues[country.CountryCode] = (confirmed < half) ? (half - confirmed) / half * double.MinValue
                                                                       : (confirmed - half) / half * double.MaxValue;
            }

            _map.HeatMap = scaledValues;

            _map.LandClick += OnUserClick;

            _page.Controls.Add(_map);
        }

        /// <summary>
        /// Subscribe an observer to be notified on click events.
        /// </summary>
        /// <param name="observer">The observer to be notified.</param>
        public void Subscribe(IMapObserver observer)
        {
            _observers.Add(observer);
        }

        /// <summary>
        /// Unsubscribe an observer from click events.
        /// </summary>
        /// <param name="observer">The observer to remove.</param>
        public void Unsubscribe(IMapObserver observer)
        {
            _observers.Remove(observer);
        }

        /// <summary>
        /// Returns the generated map control to be added in a form
        /// </summary>
        /// <returns>Generated map</returns>
        public TabPage GetPage()
        {
            return _page;
        }

        private void OnUserClick(object obj, MapData data)
        {
            CountryInfoEx? res = Utils.Find(_countries, (CountryInfoEx country) => country.CountryCode == data.Id);

            if (res.HasValue)
            {
                foreach (IMapObserver observer in _observers)
                {
                    observer.OnClick(res.Value);
                }
            }
        }

        private const string _MapFile = "World.xml";
        private IReadOnlyList<CountryInfoEx> _countries;
        private List<IMapObserver> _observers = new List<IMapObserver>();
        private GeoMap _map = new GeoMap();
        private TabPage _page = new TabPage("World Map");
    }
}
