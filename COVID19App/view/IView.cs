﻿using System.Windows.Forms;


namespace view
{
    /// <summary>
    /// Interface for uniform access to MapView, GlobalView, CountryView.
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// This method will return the control specific to a view, for now
        /// it should return a TabPage control to be added to our central
        /// TabControl
        /// </summary>
        /// <returns>The control to be inserted in the main tab control</returns>
        TabPage GetPage();
    }
}
