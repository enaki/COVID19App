﻿using core;
using database;
using view;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Windows.Forms;
using network;


namespace test_view
{
    [TestClass]
    public class TestGlobalView
    {
        // This test is used to look at page with statistics at global level.
        [TestMethod]
        public void Test()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var netProvider = new CovidDataProvider();
            var provider = new SQLiteDataProvider();

            provider.InsertCountryData(netProvider.GetCountryData());

            IReadOnlyList<CountryInfoEx> data = provider.GetCountryData();

            Form form = new Form();
            form.Width = 800;
            form.Height = 600;

            IView view = new GlobalView(data);

            TabControl tabControl = new TabControl();
            tabControl.Controls.Add(view.GetPage());

            form.Controls.Add(tabControl);
            tabControl.Dock = DockStyle.Fill;

            Application.Run(form);
        }
    }
}