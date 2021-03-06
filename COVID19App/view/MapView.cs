﻿/*************************************************************************
 *                                                                        *
 *  File:        MapView.cs                                               *
 *  Copyright:   (c) 2020, Petrica Petru                                  *
 *  E-mail:      petru.petrica@student.tuiasi.ro                          *
 *  Description: This represents the map View from the application        * 
 *                                                                        *
 *  This program is free software; you can redistribute it and/or modify  *
 *  it under the terms of the GNU General Public License as published by  *
 *  the Free Software Foundation. This program is distributed in the      *
 *  hope that it will be useful, but WITHOUT ANY WARRANTY; without even   *
 *  the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR   *
 *  PURPOSE. See the GNU General Public License for more details.         *
 *                                                                        *
 **************************************************************************/

using Core;
using LiveCharts.Maps;
using LiveCharts.WinForms;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Media;


namespace View
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

            _tableLayoutPanel.Dock = DockStyle.Fill;

            _tableLayoutPanel.RowCount = 2;
            RowStyle mapRow = new RowStyle();

            mapRow.SizeType = SizeType.Percent;
            mapRow.Height = 95;

            _tableLayoutPanel.RowStyles.Add(mapRow);

            RowStyle slideRow = new RowStyle();

            slideRow.SizeType = SizeType.Percent;
            slideRow.Height = 5;

            _tableLayoutPanel.RowStyles.Add(slideRow);

            InitializeMap(info);

            InitializeBottomLayout();

            _tableLayoutPanel.Controls.Add(_geoMap, 0, 0);

            _tableLayoutPanel.Controls.Add(_TableLayoutPanelBottom);

            _tabPage.Controls.Add(_tableLayoutPanel);
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
            return _tabPage;
        }

        private void InitializeMap(IReadOnlyList<CountryInfoEx> info)
        {
            _geoMap.Source = _MapFile;

            _geoMap.AnimationsSpeed = new TimeSpan(1000000);

            _geoMap.Dock = DockStyle.Fill;

            _geoMap.DefaultLandFill = Brushes.Gray;

            _geoMap.EnableZoomingAndPanning = true;
            _geoMap.Hoverable = true;

            GradientStopCollection collection = new GradientStopCollection();
            collection.Add(new GradientStop(Colors.MediumSeaGreen, 0.0));
            collection.Add(new GradientStop(Colors.Gold, 0.5));
            collection.Add(new GradientStop(Colors.Crimson, 1.0));

            _geoMap.GradientStopCollection = collection;

            Dictionary<string, double> scaledValues = new Dictionary<string, double>();
            foreach (CountryInfoEx country in info)
            {
                int active = country.Confirmed - country.Deaths - country.Recovered;
                double activeLog = (active > 0) ? Math.Log(active) : 0;

                scaledValues[country.CountryCode] = activeLog;
            }

            _geoMap.HeatMap = scaledValues;

            _geoMap.LandClick += OnUserClick;
        }

        private void InitializeBottomLayout()
        {
            _trackBar.Dock = DockStyle.Fill;

            _trackBar.Maximum = 0;
            _trackBar.Minimum = -_countries[0].DaysInfo.Count + 1;

            _trackBar.Value = 0;

            _trackBar.Scroll += new System.EventHandler(OnTrackBarScrolled);

            ColumnStyle dateColumn = new ColumnStyle();
            dateColumn.Width = 10;
            dateColumn.SizeType = SizeType.Percent;

            ColumnStyle trackbarColumn = new ColumnStyle();
            trackbarColumn.Width = 90;
            trackbarColumn.SizeType = SizeType.Percent;

            _TableLayoutPanelBottom.ColumnStyles.Add(trackbarColumn);
            _TableLayoutPanelBottom.ColumnStyles.Add(dateColumn);

            DayInfo dayInfo = _countries[0].DaysInfo[_countries[0].DaysInfo.Count - 1];
            _labelDate.Text = $"{dayInfo.Date.Day:00}." + $"{dayInfo.Date.Month:00}." + dayInfo.Date.Year;

            _labelDate.Dock = DockStyle.Fill;

            _labelDate.Font = new System.Drawing.Font(_labelDate.Font.FontFamily, 14);

            _TableLayoutPanelBottom.ColumnCount = 2;

            _TableLayoutPanelBottom.Controls.Add(_trackBar);
            _TableLayoutPanelBottom.Controls.Add(_labelDate);

            _TableLayoutPanelBottom.Dock = DockStyle.Fill;
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

        private void OnTrackBarScrolled(object sender, EventArgs args)
        {
            Dictionary<string, double> newValues = new Dictionary<string, double>();
            
            DayInfo info = new DayInfo();
            foreach (CountryInfoEx country in _countries)
            {
                try
                {
                    info = country.DaysInfo[country.DaysInfo.Count + _trackBar.Value - 1];

                    int active = info.Confirmed - info.Deaths - info.Recovered;
                    double activeLog = (active > 1) ? Math.Log(active) : 0;

                    newValues[country.CountryCode] = activeLog;
                } catch (Exception)
                {
                    // ignore
                }
            }

            _geoMap.HeatMap = newValues;

            _labelDate.Text = $"{info.Date.Day:00}." + $"{info.Date.Month:00}." + info.Date.Year;
        }

        private const string _MapFile = "World.xml";
 
        private IReadOnlyList<CountryInfoEx> _countries;

        private List<IMapObserver> _observers = new List<IMapObserver>();

        private TabPage _tabPage = new TabPage("World Map");
        private TableLayoutPanel _tableLayoutPanel = new TableLayoutPanel();
        private TableLayoutPanel _TableLayoutPanelBottom = new TableLayoutPanel();

        private GeoMap _geoMap = new GeoMap();
        private TrackBar _trackBar = new TrackBar();
        private Label _labelDate = new Label();
    }
}
