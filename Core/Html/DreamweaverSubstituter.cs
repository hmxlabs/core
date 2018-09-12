using System;

namespace HmxLabs.Core.Html
{
    /// <summary>
    /// A simple class that can read an Adobe Dreamweaver template file and then substitute in the value
    /// for the editable regions of the templates
    /// 
    /// The class will keep the required HTML in memory, perform the required edits and the provide
    /// the resulting HTML as a proprty to be retrieved once all the necessary edits have been performed
    /// 
    /// This class is not threadsafe.
    /// </summary>
    public class DreamweaverSubstituter
    {
        /// <summary>
        /// Construct a new instance of the class with the HTML provided as the initial HTML to operate on
        /// </summary>
        /// <param name="inputHtml_">The HTML containing Dreamweaver editable regions</param>
        public DreamweaverSubstituter(string inputHtml_)
        {
            if (null == inputHtml_)       
                throw new ArgumentNullException(nameof(inputHtml_));

            if (string.IsNullOrWhiteSpace(inputHtml_))
                throw new ArgumentException("The provided HTML is blank or whitespace only", nameof(inputHtml_));

            Html = inputHtml_;
        }

        /// <summary>
        /// The current state of the HTML. This property should be used to retrieve the HTML once all the necessary
        /// edits have been performed.
        /// </summary>
        public string Html { get; private set; }

        /// <summary>
        /// Update the HTML stored in this object to set the value of the specified editable region
        /// to the specified value
        /// </summary>
        /// <param name="editRegionName_">The name of the editable region</param>
        /// <param name="value_">The new value to set the region to</param>
        public void UpdateEditRegionValue(string editRegionName_, string value_)
        {
            Html = UpdateEditRegionValue(Html, editRegionName_, value_);
        }

        /// <summary>
        /// Update the HTML fragment provided such that the if a Dreamweaver editable region of the specified name
        /// is found it's value is changed to the provided value
        /// </summary>
        /// <param name="html_">The HTML to operate on</param>
        /// <param name="editRegionName_">The name of the editable region to change</param>
        /// <param name="value_">The new value for the editable region</param>
        /// <returns></returns>
        public static string UpdateEditRegionValue(string html_, string editRegionName_, string value_)
        {
            var startEditRegion = CreateStartEditRegionHtml(editRegionName_);
            var startIndex = html_.IndexOf(startEditRegion, StringComparison.Ordinal);
            if (0 >= startIndex)
                return html_;

            startIndex += startEditRegion.Length;
            var endIndex = html_.IndexOf(EndEditRegion, startIndex, StringComparison.Ordinal);
            var front = html_.Substring(0, startIndex);
            var back = html_.Substring(endIndex);
            return front + value_ + back;
        }

        private static string CreateStartEditRegionHtml(string regionName_)
        {
            return StartEditRegionBeg + regionName_ + StartEditRegionEnd;
        }

        private const string StartEditRegionBeg = "<!-- TemplateBeginEditable name=\"";
        private const string StartEditRegionEnd = "\" -->";
        private const string EndEditRegion = "<!-- TemplateEndEditable -->";
    }
}
